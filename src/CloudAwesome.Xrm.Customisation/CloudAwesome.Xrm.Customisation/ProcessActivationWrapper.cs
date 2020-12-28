using System;
using System.Collections.Generic;
using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Customisation.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation
{
    public class ProcessActivationWrapper
    {
        private List<string> validationErrors = new List<string>();

        public List<string> ValidateManifest(PluginManifest manifest)
        {
            // TODO - validate manifest
            return validationErrors;
        }

        public void SetStatusFromManifest(IOrganizationService client, ProcessActivationManifest manifest, ILogger logger)
        {
            var t = new TracingHelper(logger);
            t.Debug($"Entering ProcessActivationWrapper.SetStatusFromManifest");

            var activate = manifest.Status == ProcessActivationStatus.Enabled;

            // 1. Plugin steps
            if (manifest.PluginAssemblies == null) return;
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

            // 2. Workflows


            // 3. Flows


            // 4. Record Creation Rules


            // 5. Business Rules


            // 6. All Solution Components



            t.Debug($"Exiting ProcessActivationWrapper.SetStatusFromManifest");
        }

    }
}
