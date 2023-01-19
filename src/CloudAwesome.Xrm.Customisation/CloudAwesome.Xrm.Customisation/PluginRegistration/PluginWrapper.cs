using System.Linq;
using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Core.Loggers;
using CloudAwesome.Xrm.Customisation.Exceptions;
using CloudAwesome.Xrm.Customisation.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation.PluginRegistration
{
    public class PluginWrapper
    {
        public static ManifestValidationResult Validate(PluginManifest manifest, 
            bool throwOnError = false, TracingHelper t = null)
        {
            var validator = new PluginManifestValidator();
            var result = validator.Validate(manifest);

            if (result.IsValid)
                return new ManifestValidationResult()
                {
                    IsValid = true
                };
            
            if (!throwOnError)
                return new ManifestValidationResult()
                {
                    IsValid = false,
                    Errors = result.Errors.Select(e => e.ErrorMessage)
                };
            {
                var errorsList = string.Join(",", result.Errors);
                
                t.Critical($"Manifest is invalid and has {result.Errors.Count()} errors: {errorsList}");
                throw new InvalidManifestException(errorsList);
            }
        }
        
        /// <summary>
        /// Loops through each PluginAssembly in the manifest and registers all assemblies, plugins, steps and images
        /// </summary>
        /// <param name="manifest">XML plugin manifest</param>
        /// <param name="client">IOrganizationService client reference</param>
        public void RegisterPlugins(PluginManifest manifest, IOrganizationService client)
        {
            if (manifest.LoggingConfiguration != null)
            {
                var t = new TracingHelper(manifest.LoggingConfiguration);
                RegisterPlugins(manifest, client, t);
            }
            else
            {
                // Default to a console logger if all else fails
                ILogger consoleLogger = new ConsoleLogger(LogLevel.Information);
                var t = new TracingHelper(consoleLogger);
                RegisterPlugins(manifest, client, t);
            }
        }

        /// <summary>
        /// Loops through each PluginAssembly in the manifest and registers all assemblies, plugins, steps and images.
        /// Accepts custom ILogger implementation for custom logging output 
        /// </summary>
        /// <param name="manifest"></param>
        /// <param name="client"></param>
        /// <param name="logger"></param>
        public void RegisterPlugins(PluginManifest manifest, IOrganizationService client, ILogger logger)
        {
            var t = new TracingHelper(logger);
            RegisterPlugins(manifest, client, t);
        }

        /// <summary>
        /// Loops through each PluginAssembly in the manifest and registers all assemblies, plugins, steps and images
        /// </summary>
        /// <param name="manifest">XML plugin manifest</param>
        /// <param name="client">IOrganizationService client reference</param>
        /// <param name="t">Tracing helpers for logging</param>
        public void RegisterPlugins(PluginManifest manifest, IOrganizationService client, TracingHelper t)
        {
            t.Debug($"Entering PluginWrapper.RegisterPlugins");
            var manifestValidation = Validate(manifest, true, t);
            
            if (manifest.Clobber)
            {
                t.Warning($"Manifest has 'Clobber' set to true. Deleting referenced Plugins before re-registering");
                //this.UnregisterServiceEndPoints(manifest, client, t);
                UnregisterPlugins.Run(manifest, client, t);
            }
            
            foreach (var pluginAssembly in manifest.PluginAssemblies)
            {
                var targetSolutionName = SolutionWrapper.DefineSolutionNameFromManifest(manifest, pluginAssembly);
                var createdAssembly = RegisterPluginAssembly.Run(client, manifest, pluginAssembly, targetSolutionName, t);
                if (manifest.UpdateAssemblyOnly) continue;
                
                foreach (var plugin in pluginAssembly.Plugins)
                {
                    var createdPluginType = RegisterPluginType.Run(plugin, createdAssembly, client, t);

                    if (plugin.Steps != null)
                    {
                        foreach (var pluginStep in plugin.Steps)
                        {
                            var createdStep = RegisterPluginStep.Run(pluginStep, createdPluginType, targetSolutionName, client, t);
                            
                            if (pluginStep.EntityImages == null) continue;
                            foreach (var entityImage in pluginStep.EntityImages)
                            {
                                var image = RegisterEntityImage.Run(entityImage, createdStep, client, t);
                            }
                        }
                    }

                    if (plugin.CustomApis == null) continue;
                    foreach (var api in plugin.CustomApis)
                    {
                        var createdApi = RegisterCustomApi.Run(api, createdPluginType, targetSolutionName, client, t);
                        
                        if (api.RequestParameters != null)
                        {
                            foreach (var requestParameter in api.RequestParameters)
                            {
                                var createdRequestParameter = RegisterCustomApiRequestParameter.Run(requestParameter,
                                    createdApi, targetSolutionName, client, t);
                            }
                        }

                        if (api.ResponseProperties != null)
                        {
                            foreach (var responseProperty in api.ResponseProperties)
                            {
                                var createdResponseProperty = RegisterCustomerApiResponseProperty.Run(responseProperty,
                                    createdApi, targetSolutionName, client, t);
                            }
                        }
                    }
                }
            }
            
            t.Debug($"Exiting PluginWrapper.RegisterPlugins");
        }
        
        public void RegisterServiceEndpoints(PluginManifest manifest, IOrganizationService client)
        {
            if (manifest.LoggingConfiguration != null)
            {
                var t = new TracingHelper(manifest.LoggingConfiguration);
                RegisterServiceEndpoints(manifest, client, t);
            }
            else
            {
                RegisterServiceEndpoints(manifest, client, t: null);
            }
        }

        public void RegisterServiceEndpoints(PluginManifest manifest, IOrganizationService client, ILogger logger)
        {
            var t = new TracingHelper(logger);
            RegisterServiceEndpoints(manifest, client, t);
        }

        public void RegisterServiceEndpoints(PluginManifest manifest, IOrganizationService client, TracingHelper t)
        {
            if (manifest.ServiceEndpoints == null || manifest.ServiceEndpoints.Length == 0) return;
            t.Debug($"Entering PluginWrapper.RegisterServiceEndpoints");

            if (manifest.Clobber)
            {
                t.Warning($"Manifest has 'Clobber' set to true. Deleting referenced Plugins before re-registering");
                this.UnregisterServiceEndPoints(manifest, client, t);
            }

            // 1. Register Service Endpoints
            foreach (var serviceEndpoint in manifest.ServiceEndpoints)
            {
                t.Debug($"Registering Assembly {serviceEndpoint.Name}");
                var createdEndpoint = serviceEndpoint.Register(client);
                t.Info($"Assembly {serviceEndpoint.Name} registered with ID {createdEndpoint.Id}");

                SolutionWrapper.AddSolutionComponent(client, manifest.SolutionName, createdEndpoint.Id, ComponentType.ServiceEndpoint);
                t.Debug($"Plugin Step {serviceEndpoint.Name} added to solution {manifest.SolutionName}");

                // 2. Register Steps
                foreach (var step in serviceEndpoint.Steps)
                {
                    t.Debug($"Processing Step = {step.FriendlyName}");

                    var sdkMessage = PluginQueries.GetSdkMessageQuery(step.Message).RetrieveSingleRecord(client);
                    var sdkMessageFilter = 
                        PluginQueries.GetSdkMessageFilterQuery(step.PrimaryEntity, sdkMessage.Id)
                            .RetrieveSingleRecord(client);

                    var createdStep = 
                        step.Register(client, createdEndpoint, 
                            sdkMessage.ToEntityReference(), sdkMessageFilter.ToEntityReference());
                    t.Info($"Plugin step {step.FriendlyName} registered with ID {createdStep.Id}");

                    SolutionWrapper.AddSolutionComponent(client, manifest.SolutionName, 
                        createdStep.Id, ComponentType.SdkMessageProcessingStep);

                    // 3. Register Entity Images
                    if (step.EntityImages == null) continue;
                    foreach (var entityImage in step.EntityImages)
                    {
                        t.Debug($"Processing Entity Image = {entityImage.Name}");
                        var createdImage = entityImage.Register(client, createdStep);
                        t.Info($"Entity image {entityImage.Name} registered with ID {createdImage}");
                    }
                }
            }

            t.Debug($"Exiting PluginWrapper.RegisterServiceEndpoints");
        }

        public void UnregisterServiceEndPoints(PluginManifest manifest, IOrganizationService client)
        {
            if (manifest.LoggingConfiguration != null)
            {
                var t = new TracingHelper(manifest.LoggingConfiguration);
                UnregisterServiceEndPoints(manifest, client, t);
            }
            else
            {
                UnregisterServiceEndPoints(manifest, client, t: null);
            }
        }

        public void UnregisterServiceEndPoints(PluginManifest manifest, IOrganizationService client, ILogger logger)
        {
            var t = new TracingHelper(logger);
            UnregisterServiceEndPoints(manifest, client, t);
        }

        public void UnregisterServiceEndPoints(PluginManifest manifest, IOrganizationService client, TracingHelper t)
        {
            if (manifest.ServiceEndpoints == null || manifest.ServiceEndpoints.Length == 0) return;
            t.Debug($"Entering PluginWrapper.UnregisterServiceEndPoints");

            foreach (var serviceEndpoint in manifest.ServiceEndpoints)
            {
                var existingEndpoint = serviceEndpoint.GetExistingServiceEndpoint().RetrieveSingleRecord(client);
                if (existingEndpoint == null) return;

                var childStepsResults =
                    PluginQueries.GetChildPluginStepsQuery(existingEndpoint.ToEntityReference()).RetrieveMultiple(client);
                var childStepsList = childStepsResults.Entities.Select(e => e.Id).ToList();

                if (childStepsList.Count > 0)
                {
                    PluginQueries.GetChildEntityImagesQuery(childStepsList).DeleteAllResults(client);
                }

                PluginQueries.GetChildPluginStepsQuery(existingEndpoint.ToEntityReference()).DeleteAllResults(client);
                serviceEndpoint.GetExistingServiceEndpoint().DeleteSingleRecord(client);
            }

            t.Debug($"Exiting PluginWrapper.UnregisterServiceEndPoints");
        }
        
    }
}
