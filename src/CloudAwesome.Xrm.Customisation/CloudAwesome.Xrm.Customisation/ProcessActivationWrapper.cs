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

            foreach (var assembly in manifest.PluginAssemblies)
            {
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

            t.Debug($"Exiting ProcessActivationWrapper.SetStatusFromManifest");
        }

    }
}
