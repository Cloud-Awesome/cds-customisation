using System;
using System.IO;
using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Customisation.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Customisation
{
    public class PluginWrapper
    {
        /// <summary>
        /// Loops through each PluginAssembly in the manifest and registers all assemblies, plugins, steps and images
        /// </summary>
        /// <param name="manifest">XML plugin manifest</param>
        /// <param name="client">IOrganizationService client reference</param>
        public void RegisterPlugins(PluginManifest manifest, IOrganizationService client)
        {
            RegisterPlugins(manifest, client, null);
        }

        /// <summary>
        /// Loops through each PluginAssembly in the manifest and registers all assemblies, plugins, steps and images
        /// </summary>
        /// <param name="manifest">XML plugin manifest</param>
        /// <param name="client">IOrganizationService client reference</param>
        /// <param name="logger">Optional ILogger implementation</param>
        public void RegisterPlugins(PluginManifest manifest, IOrganizationService client, ILogger logger)
        {
            var t = new TracingHelper(logger);
            t.Debug($"Entering PluginWrapper.RegisterPlugins");

            // 1. Register Assemblies
            foreach (var pluginAssembly in manifest.PluginAssemblies)
            {
                var targetSolutionName = string.Empty;
                if (!string.IsNullOrEmpty(pluginAssembly.SolutionName))
                {
                    targetSolutionName = pluginAssembly.SolutionName;

                }
                else if (!string.IsNullOrEmpty(manifest.SolutionName))
                {
                    targetSolutionName = manifest.SolutionName;
                }

                t.Debug($"Processing Assembly FriendlyName = {pluginAssembly.FriendlyName}");
                var pluginAssemblyInfo = new PluginAssemblyInfo(pluginAssembly.Assembly);

                var assemblyEntity = new PluginAssembly()
                {
                    Name = pluginAssembly.Name,
                    Culture = pluginAssemblyInfo.Culture,
                    Version = pluginAssemblyInfo.Version,
                    PublicKeyToken = pluginAssemblyInfo.PublicKeyToken,
                    SourceType = PluginAssembly_SourceType.Database,// Only database supported now
                    IsolationMode = PluginAssembly_IsolationMode.Sandbox, // Only Sandbox supported now
                    Content = Convert.ToBase64String(File.ReadAllBytes(pluginAssembly.Assembly))
                };
                
                var createdAssembly = new EntityReference(PluginAssembly.EntityLogicalName,
                    assemblyEntity.CreateOrUpdate(client, GetExistingAssemblyQuery(pluginAssembly.Name, pluginAssemblyInfo.Version)));
                t.Info($"Assembly {pluginAssembly.FriendlyName} registered with ID {createdAssembly.Id}");

                SolutionWrapper.AddSolutionComponent(client, targetSolutionName, createdAssembly.Id, ComponentType.PluginAssembly);
                t.Debug($"Assembly {pluginAssembly.FriendlyName} added to solution {targetSolutionName}");

                // 2. Register Plugins
                foreach (var plugin in pluginAssembly.Plugins)
                {
                    t.Info($"PluginType FriendlyName = {plugin.FriendlyName}");

                    var pluginType = new PluginType()
                    {
                        PluginAssemblyId = createdAssembly,
                        TypeName = plugin.Name,
                        FriendlyName = plugin.FriendlyName,
                        Name = plugin.Name,
                        Description = plugin.Description
                    };

                    var createdPluginType = new EntityReference(PluginType.EntityLogicalName,
                        pluginType.CreateOrUpdate(client, GetExistingPluginQuery(plugin.Name, createdAssembly.Id)));
                    t.Info($"Plugin {plugin.FriendlyName} registered with ID {createdPluginType.Id}");

                    if (plugin.Steps == null) continue;

                    // 3. Register Plugin Steps
                    foreach (var pluginStep in plugin.Steps)
                    {
                        t.Debug($"Processing Step = {pluginStep.FriendlyName}");

                        var sdkMessage = GetSdkMessageQuery(pluginStep.Message).RetrieveSingleRecord(client);
                        var sdkStep = new SdkMessageProcessingStep()
                        {
                            Name = pluginStep.Name,
                            Configuration = pluginStep.UnsecureConfiguration,
                            Mode = pluginStep.ExecutionMode,
                            Rank = pluginStep.ExecutionOrder,
                            Stage = pluginStep.Stage,
                            SupportedDeployment = SdkMessageProcessingStep_SupportedDeployment.ServerOnly, // Only ServerOnly supported
                            EventHandler = createdPluginType,
                            SdkMessageId = sdkMessage.ToEntityReference(),
                            Description = pluginStep.Description,
                            AsyncAutoDelete = pluginStep.AsyncAutoDelete
                            // TODO loop through attributes to create a single string?
                            //FilteringAttributes = step.FilteringAttributes.
                        };

                        var createdStep = new EntityReference(SdkMessageProcessingStep.EntityLogicalName,
                            sdkStep.CreateOrUpdate(client, GetExistingPluginStepQuery(createdPluginType.Id, sdkStep.Id)));
                        t.Info($"Plugin step {pluginStep.FriendlyName} registered with ID {createdStep.Id}");

                        SolutionWrapper.AddSolutionComponent(client, targetSolutionName,
                            createdStep.Id, ComponentType.SDKMessageProcessingStep);
                        t.Debug($"Plugin Step {pluginStep.FriendlyName} added to solution {targetSolutionName}");

                        // 4. Register Entity Images
                        foreach (var entityImage in pluginStep.EntityImages)
                        {
                            t.Debug($"Processing Entity Image = {entityImage.Name}");

                            var stepImage = new SdkMessageProcessingStepImage()
                            {
                                Name = entityImage.Name,
                                EntityAlias = entityImage.Name,
                                Attributes1 = string.Join(",", entityImage.Attributes),
                                ImageType = SdkMessageProcessingStepImage_ImageType.PreImage,
                                MessagePropertyName = "Target",
                                SdkMessageProcessingStepId = createdStep
                            };

                            var createdImage = stepImage.CreateOrUpdate(client,
                                GetExistingEntityImageQuery(entityImage.Name, createdStep.Id));
                            t.Info($"Entity image {entityImage.Name} registered with ID {createdImage}");

                        }
                    }
                }
            }
            t.Debug($"Exiting PluginWrapper.RegisterPlugins");
        }

        private static QueryBase GetExistingAssemblyQuery(string assemblyName, string assemblyVersion)
        {
            return new QueryExpression()
            {
                EntityName = PluginAssembly.EntityLogicalName,
                ColumnSet = new ColumnSet(PluginAssembly.PrimaryIdAttribute, PluginAssembly.PrimaryNameAttribute),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(PluginAssembly.PrimaryNameAttribute, ConditionOperator.Equal, assemblyName),
                        new ConditionExpression("version", ConditionOperator.Equal, assemblyVersion)
                    }
                }
            };
        }

        private static QueryBase GetExistingPluginQuery(string pluginName, Guid parentAssemblyId)
        {
            return new QueryExpression()
            {
                EntityName = PluginType.EntityLogicalName,
                ColumnSet = new ColumnSet(PluginType.PrimaryIdAttribute, PluginType.PrimaryNameAttribute),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(PluginType.PrimaryNameAttribute, ConditionOperator.Equal, pluginName),
                        new ConditionExpression("pluginassemblyid", ConditionOperator.Equal, parentAssemblyId)
                    }
                }
            };
        }

        private static QueryBase GetExistingPluginStepQuery(Guid parentPluginType, Guid sdkMessage)
        {
            return new QueryExpression(SdkMessageProcessingStep.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(SdkMessageProcessingStep.PrimaryIdAttribute,
                    SdkMessageProcessingStep.PrimaryNameAttribute),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression("eventhandler", ConditionOperator.Equal, parentPluginType),
                        new ConditionExpression("sdkmessageid", ConditionOperator.Equal, sdkMessage),
                        new ConditionExpression("stage", ConditionOperator.Equal,
                            (int)SdkMessageProcessingStep_Stage.Postoperation),
                    }
                }
            };
        }

        private static QueryBase GetExistingEntityImageQuery(string entityImageName, Guid parentPluginStep)
        {
            return new QueryExpression(SdkMessageProcessingStepImage.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(SdkMessageProcessingStepImage.PrimaryIdAttribute,
                    SdkMessageProcessingStep.PrimaryNameAttribute),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageProcessingStepImage.PrimaryNameAttribute,
                            ConditionOperator.Equal, entityImageName),
                        new ConditionExpression(SdkMessageProcessingStep.PrimaryIdAttribute,
                            ConditionOperator.Equal, parentPluginStep)
                    }
                }
            };
        }

        private static QueryBase GetSdkMessageQuery(string sdkMessageName)
        {
            return new QueryExpression(SdkMessage.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(SdkMessage.PrimaryIdAttribute, SdkMessage.PrimaryNameAttribute),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessage.PrimaryNameAttribute, ConditionOperator.Equal,
                            sdkMessageName)
                    }
                }
            };
        }

    }
}
