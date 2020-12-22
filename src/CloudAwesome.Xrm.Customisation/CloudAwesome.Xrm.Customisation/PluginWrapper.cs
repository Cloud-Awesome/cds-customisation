using System;
using System.Collections.Generic;
using System.Linq;
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

            // TODO - verify existence all messages, entities and filters before continue #4
            // TODO - if queried messages etc above, then you can extract the queries in loops below to save on hits to the API
            
            if (manifest.Clobber)
            {
                t.Info($"Manifest has 'Clobber' set to true. Deleting referenced Plugins before re-registering");
                this.UnregisterPlugins(manifest, client, logger);
            }

            // 1. Register Assemblies
            foreach (var pluginAssembly in manifest.PluginAssemblies)
            {
                t.Debug($"Processing Assembly FriendlyName = {pluginAssembly.FriendlyName}");

                var targetSolutionName = string.Empty;
                if (!string.IsNullOrEmpty(pluginAssembly.SolutionName))
                {
                    targetSolutionName = pluginAssembly.SolutionName;
                }
                else if (!string.IsNullOrEmpty(manifest.SolutionName))
                {
                    targetSolutionName = manifest.SolutionName;
                }

                t.Debug($"Registering Assembly {pluginAssembly.FriendlyName}");
                var createdAssembly = pluginAssembly.Register(client);
                t.Info($"Assembly {pluginAssembly.FriendlyName} registered with ID {createdAssembly.Id}");

                SolutionWrapper.AddSolutionComponent(client, targetSolutionName, createdAssembly.Id, ComponentType.PluginAssembly);
                t.Debug($"Assembly {pluginAssembly.FriendlyName} added to solution {targetSolutionName}");

                // TODO - manifest flag to update assembly code only (and cut out here int the loop. #2

                // 2. Register Plugins
                foreach (var plugin in pluginAssembly.Plugins)
                {
                    t.Debug($"PluginType FriendlyName = {plugin.FriendlyName}");
                    var createdPluginType = plugin.Register(client, createdAssembly);
                    t.Info($"Plugin {plugin.FriendlyName} registered with ID {createdPluginType.Id}");

                    if (plugin.Steps == null) continue;

                    // 3. Register Plugin Steps
                    foreach (var pluginStep in plugin.Steps)
                    {
                        t.Debug($"Processing Step = {pluginStep.FriendlyName}");

                        var sdkMessage = GetSdkMessageQuery(pluginStep.Message).RetrieveSingleRecord(client);
                        var sdkMessageFilter = GetSdkMessageFilter(pluginStep.PrimaryEntity, sdkMessage.Id).RetrieveSingleRecord(client);

                        var createdStep = pluginStep.Register(client, createdPluginType, sdkMessage.ToEntityReference(), sdkMessageFilter.ToEntityReference());
                        t.Info($"Plugin step {pluginStep.FriendlyName} registered with ID {createdStep.Id}");

                        SolutionWrapper.AddSolutionComponent(client, targetSolutionName, createdStep.Id, ComponentType.SDKMessageProcessingStep);
                        t.Debug($"Plugin Step {pluginStep.FriendlyName} added to solution {targetSolutionName}");

                        // 4. Register Entity Images
                        if (pluginStep.EntityImages == null) continue;
                        foreach (var entityImage in pluginStep.EntityImages)
                        {
                            t.Debug($"Processing Entity Image = {entityImage.Name}");
                            var createdImage = entityImage.Register(client, createdStep);
                            t.Info($"Entity image {entityImage.Name} registered with ID {createdImage}");

                        }
                    }
                }
            }
            t.Debug($"Exiting PluginWrapper.RegisterPlugins");
        }

        public void UnregisterPlugins(PluginManifest manifest, IOrganizationService client)
        {
            UnregisterPlugins(manifest, client, null);
        }

        public void UnregisterPlugins(PluginManifest manifest, IOrganizationService client, ILogger logger)
        {
            // God awful initial version in need of some refactoring!! :)
            // TODO - Cut out if no child are returned (e.g. no images) #9

            var t = new TracingHelper(logger);
            t.Debug($"Entering PluginWrapper.UnregisterPlugins");
            
            foreach (var pluginAssembly in manifest.PluginAssemblies)
            {
                var pluginAssemblyInfo = new PluginAssemblyInfo(pluginAssembly.Assembly);
                var existingAssembly = pluginAssembly.GetExistingQuery(pluginAssemblyInfo.Version).RetrieveSingleRecord(client);

                if (existingAssembly == null) return;

                var childPluginTypesResults = GetChildPluginTypes(client, existingAssembly.ToEntityReference()).RetrieveMultiple(client);
                var pluginsList = childPluginTypesResults.Entities.Select(e => e.Id).ToList();

                var childStepsResults = GetChildPluginSteps(client, pluginsList).RetrieveMultiple(client);
                var pluginStepsList = childStepsResults.Entities.Select(e => e.Id).ToList();

                GetChildEntityImages(client, pluginStepsList).DeleteAllResults(client);

                GetChildPluginSteps(client, pluginsList).DeleteAllResults(client);
                GetChildPluginTypes(client, existingAssembly.ToEntityReference()).DeleteAllResults(client);
                pluginAssembly.GetExistingQuery(pluginAssemblyInfo.Version).DeleteSingleRecord(client);

            }
            t.Debug($"Exiting PluginWrapper.UnregisterPlugins");
        }

        private static QueryBase GetSdkMessageQuery(string sdkMessageName)
        {
            return new QueryExpression(SdkMessage.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(SdkMessage.PrimaryIdAttribute, 
                    SdkMessage.PrimaryNameAttribute),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessage.PrimaryNameAttribute, 
                            ConditionOperator.Equal, sdkMessageName)
                    }
                }
            };
        }

        private static QueryBase GetSdkMessageFilter(string entityName, Guid sdkMessageId)
        {
            return new QueryExpression(SdkMessageFilter.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(SdkMessageFilter.Fields.Name, 
                    SdkMessageFilter.Fields.PrimaryObjectTypeCode, SdkMessageFilter.Fields.SdkMessageId),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageFilter.Fields.PrimaryObjectTypeCode, 
                            ConditionOperator.Equal, entityName),
                        new ConditionExpression(SdkMessageFilter.Fields.SdkMessageId, 
                            ConditionOperator.Equal, sdkMessageId.ToString())
                    }
                }
            };
        }

        private static QueryBase GetChildPluginTypes(IOrganizationService client, EntityReference existingAssembly)
        {
            return new QueryByAttribute()
            {
                EntityName = PluginType.EntityLogicalName,
                ColumnSet = new ColumnSet(PluginType.PrimaryIdAttribute,
                    PluginType.PrimaryNameAttribute),
                Attributes = { PluginType.Fields.PluginAssemblyId },
                Values = { existingAssembly.Id }
            };
        }

        private static QueryBase GetChildPluginSteps(IOrganizationService client, List<Guid> parentPluginsList)
        {
            return new QueryExpression()
            {
                EntityName = SdkMessageProcessingStep.EntityLogicalName,
                ColumnSet = new ColumnSet(SdkMessageProcessingStep.PrimaryIdAttribute,
                    SdkMessageProcessingStep.PrimaryNameAttribute),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageProcessingStep.Fields.PluginTypeId,
                            ConditionOperator.In, parentPluginsList)
                    }
                }
            };
        }

        private static QueryBase GetChildEntityImages(IOrganizationService client, List<Guid> parentStepsList)
        {
            return new QueryExpression()
            {
                EntityName = SdkMessageProcessingStepImage.EntityLogicalName,
                ColumnSet = new ColumnSet(SdkMessageProcessingStepImage.PrimaryIdAttribute,
                    SdkMessageProcessingStepImage.PrimaryNameAttribute),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageProcessingStepImage.Fields.SdkMessageProcessingStepId,
                            ConditionOperator.In, parentStepsList)
                    }
                }
            };
        }
    }
}
