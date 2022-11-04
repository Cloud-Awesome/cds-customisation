using System;
using System.Collections.Generic;
using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Customisation.EarlyBoundModels;
using CloudAwesome.Xrm.Customisation.Exceptions;
using CloudAwesome.Xrm.Customisation.PluginRegistration;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Customisation.ProcessActivation
{
    public class ProcessActivationWrapper
    {
        private List<string> validationErrors = new List<string>();

        public List<string> ValidateManifest(PluginManifest manifest)
        {
            // TODO - validate manifest
            return validationErrors;
        }

        public void SetStatusFromManifest(IOrganizationService client, ProcessActivationManifest manifest)
        {
            if (manifest.LoggingConfiguration != null)
            {
                var t = new TracingHelper(manifest.LoggingConfiguration);
                SetStatusFromManifest(manifest, client, t);
            }
            else
            {
                SetStatusFromManifest(manifest, client, t: null);
            }
        }

        public void SetStatusFromManifest(ProcessActivationManifest manifest, IOrganizationService client, ILogger logger)
        {
            var t = new TracingHelper(logger);
            SetStatusFromManifest(manifest, client, t);
        }

        public void SetStatusFromManifest(ProcessActivationManifest manifest, IOrganizationService client, TracingHelper t)
        {
            t.Debug($"Entering ProcessActivationWrapper.SetStatusFromManifest");

            var activate = manifest.Status == ProcessActivationStatus.Enabled;
            var targetState = activate
                ? new OptionSetValue((int)WorkflowState.Activated)
                : new OptionSetValue((int)WorkflowState.Draft);
            var targetStatus = activate
                ? new OptionSetValue((int)Workflow_StatusCode.Activated)
                : new OptionSetValue((int)Workflow_StatusCode.Draft);
            var traceMessagePrefix = activate
                ? "Enabling"
                : "Deactivating";
            
            t.Info($"{traceMessagePrefix} processes");

            // 6. All Solution Components
            if (manifest.Solutions == null) return;
            foreach (var solution in manifest.Solutions)
            {
                t.Info($"{traceMessagePrefix} processes in solution: '{solution.Name}'");
                
                // Temp only for Altus go live!
                
                var solutionRecords = this.GetCloudFlowsFromSolution(client, solution.Name);
                t.Info($"Retrieved {solutionRecords.Entities.Count} processes in this solution");
                var i = 0;
                
                foreach (var entity in solutionRecords.Entities)
                {
                    var workflow = (Workflow) new Workflow(Guid.Parse(entity["objectid"].ToString()))
                        .Retrieve(client);
                    i++;

                    try
                    {
                        var setstate = new SetStateRequest()
                        {
                            EntityMoniker = new EntityReference(workflow.LogicalName, workflow.Id),
                            State = targetState,
                            Status = targetStatus
                        };
                        
                        client.Execute(setstate);

                        t.Info($"{i}/{solutionRecords.Entities.Count}. {workflow.Name} updated");
                    }
                    catch (Exception e)
                    {
                        t.Error($"*** Failed to set: {i}/{solutionRecords.Entities.Count}. {workflow.Name}, ({e.Message})");
                    }
                }
                
                // ^^ Temp only for Altus go live!
                
                
                
                /*
                 * 1. Get Cloud Flow from solution -> Set status
                 * 2. Get Plugin steps from solution -> Set status
                 * 3. Get Classic Workflows from solution -> Set status
                 * 4. Get Business Rules from solution -> Set status
                 * 5. Get Case Creation Rules from solution -> Set status
                */
                
                
                
            }

            // 1. Plugin steps
            /*if (manifest.PluginAssemblies == null) return;
            foreach (var assembly in manifest.PluginAssemblies)
            {
                t.Info($"Processing plugin step {assembly.Name}");

                if (assembly.AllChildren)
                {
                    t.Debug($"Setting all grandchild steps to enabled = {activate}");
                    // 1. Query all grandchild steps

                    
                    // 2. set state for all in bulk

                    t.Info($"All grandchild steps processed");
                    continue;
                }

                foreach (var step in assembly.Steps)
                {
                    t.Info($"Processing plugin step {step.Name}");
                    try
                    {
                        step.ToggleStatus(client, activate);
                        t.Debug($"Plugin step {step.Name} successfully processed");
                    }
                    catch (NoProcessToUpdateException e)
                    {
                        t.Warning(e.Message);
                    }
                    catch (Exception e)
                    {
                        t.Critical($"Unexpected exception thrown: {e.Message}");
                        throw;
                    }
                }
            }
            */
            // 2. Classic Workflows
            // 3. Modern Flows
            // 4. Record Creation Rules
            // 5. Business Rules (Entities)
            
            t.Info($"Exiting ProcessActivationWrapper.SetStatusFromManifest");
        }

        public EntityCollection GetCloudFlowsFromSolution(IOrganizationService client, string solutionName)
        {
            return SolutionWrapper.RetrieveSolutionComponents(client, solutionName, ComponentType.Workflow);
        }

    }
}
