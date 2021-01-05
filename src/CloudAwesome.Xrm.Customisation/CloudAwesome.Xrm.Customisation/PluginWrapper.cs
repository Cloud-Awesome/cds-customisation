﻿using System;
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
            if (manifest.LoggingConfiguration != null)
            {
                var t = new TracingHelper(manifest.LoggingConfiguration);
                RegisterPlugins(manifest, client, t);
            }
            else
            {
                RegisterPlugins(manifest, client, t: null);
            }
        }

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

            if (manifest.Clobber)
            {
                t.Info($"Manifest has 'Clobber' set to true. Deleting referenced Plugins before re-registering");
                this.UnregisterPlugins(manifest, client, t);
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

                if (manifest.UpdateAssemblyOnly) continue;

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
                        var sdkMessageFilter = GetSdkMessageFilterQuery(pluginStep.PrimaryEntity, sdkMessage.Id).RetrieveSingleRecord(client);

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
                            t.Info($"Entity image {entityImage.Name} registered with ID {createdImage.Id}");

                        }
                    }
                }
            }

            

            t.Debug($"Exiting PluginWrapper.RegisterPlugins");
        }

        public void UnregisterPlugins(PluginManifest manifest, IOrganizationService client)
        {
            if (manifest.LoggingConfiguration != null)
            {
                var t = new TracingHelper(manifest.LoggingConfiguration);
                UnregisterPlugins(manifest, client, t);
            }
            else
            {
                UnregisterPlugins(manifest, client, t: null);
            }
        }

        public void UnregisterPlugins(PluginManifest manifest, IOrganizationService client, ILogger logger)
        {
            var t = new TracingHelper(logger);
            UnregisterPlugins(manifest, client, t);
        }

        public void UnregisterPlugins(PluginManifest manifest, IOrganizationService client, TracingHelper t)
        {
            t.Debug($"Entering PluginWrapper.UnregisterPlugins");

            foreach (var pluginAssembly in manifest.PluginAssemblies)
            {
                var pluginAssemblyInfo = new PluginAssemblyInfo(pluginAssembly.Assembly);
                var existingAssembly = pluginAssembly.GetExistingQuery(pluginAssemblyInfo.Version).RetrieveSingleRecord(client);

                if (existingAssembly == null) return;

                var childPluginTypesResults = GetChildPluginTypesQuery(existingAssembly.ToEntityReference()).RetrieveMultiple(client);
                var pluginsList = childPluginTypesResults.Entities.Select(e => e.Id).ToList();

                var childStepsResults = GetChildPluginStepsQuery(pluginsList).RetrieveMultiple(client);
                var pluginStepsList = childStepsResults.Entities.Select(e => e.Id).ToList();

                if (pluginStepsList.Count > 0)
                {
                    GetChildEntityImagesQuery(pluginStepsList).DeleteAllResults(client);
                }

                if (pluginsList.Count > 0)
                {
                    GetChildPluginStepsQuery(pluginsList).DeleteAllResults(client);
                }

                GetChildPluginTypesQuery(existingAssembly.ToEntityReference()).DeleteAllResults(client);
                pluginAssembly.GetExistingQuery(pluginAssemblyInfo.Version).DeleteSingleRecord(client);

            }

            t.Debug($"Exiting PluginWrapper.UnregisterPlugins");
        }
        
        public void RegisterServiceEndpoints(PluginManifest manifest, IOrganizationService client)
        {
            RegisterServiceEndpoints(manifest, client, null);
        }

        public void RegisterServiceEndpoints(PluginManifest manifest, IOrganizationService client, ILogger logger)
        {
            var t = new TracingHelper(logger);
            t.Debug($"Entering PluginWrapper.RegisterServiceEndpoints");

            if (manifest.Clobber)
            {
                t.Info($"Manifest has 'Clobber' set to true. Deleting referenced Plugins before re-registering");
                this.UnregisterServiceEndPoints(manifest, client, logger);
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

                    var sdkMessage = GetSdkMessageQuery(step.Message).RetrieveSingleRecord(client);
                    var sdkMessageFilter = GetSdkMessageFilterQuery(step.PrimaryEntity, sdkMessage.Id).RetrieveSingleRecord(client);

                    var createdStep = step.Register(client, createdEndpoint, sdkMessage.ToEntityReference(), sdkMessageFilter.ToEntityReference());
                    t.Info($"Plugin step {step.FriendlyName} registered with ID {createdStep.Id}");

                    if (manifest.SolutionName != null)
                    {
                        SolutionWrapper.AddSolutionComponent(client, manifest.SolutionName, createdStep.Id, ComponentType.SDKMessageProcessingStep);
                        t.Debug($"Plugin Step {step.FriendlyName} added to solution {manifest.SolutionName}");
                    }

                    // 3. register Entity Images
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
            UnregisterServiceEndPoints(manifest, client, null);
        }

        public void UnregisterServiceEndPoints(PluginManifest manifest, IOrganizationService client, ILogger logger)
        {
            var t = new TracingHelper(logger);
            t.Debug($"Entering PluginWrapper.UnregisterServiceEndPoints");

            foreach (var serviceEndpoint in manifest.ServiceEndpoints)
            {
                var existingEndpoint = serviceEndpoint.GetExistingServiceEndpoint().RetrieveSingleRecord(client);
                if (existingEndpoint == null) return;

                var childStepsResults =
                    GetChildPluginStepsQuery(existingEndpoint.ToEntityReference()).RetrieveMultiple(client);
                var childStepsList = childStepsResults.Entities.Select(e => e.Id).ToList();

                if (childStepsList.Count > 0)
                {
                    GetChildEntityImagesQuery(childStepsList).DeleteAllResults(client);
                }

                GetChildPluginStepsQuery(existingEndpoint.ToEntityReference()).DeleteAllResults(client);
                serviceEndpoint.GetExistingServiceEndpoint().DeleteSingleRecord(client);
            }

            t.Debug($"Exiting PluginWrapper.UnregisterServiceEndPoints");
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

        private static QueryBase GetSdkMessageFilterQuery(string entityName, Guid sdkMessageId)
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

        private static QueryBase GetChildPluginTypesQuery(EntityReference existingAssembly)
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

        private static QueryBase GetChildPluginStepsQuery(List<Guid> parentPluginsList)
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

        private static QueryBase GetChildPluginStepsQuery(EntityReference existingEndpoint)
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
                        new ConditionExpression(SdkMessageProcessingStep.Fields.EventHandler, 
                            ConditionOperator.Equal, existingEndpoint.Id)
                    }
                }
            };
        }

        private static QueryBase GetChildEntityImagesQuery(List<Guid> parentStepsList)
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
