using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation
{
    public class ConfigurationWrapper
    {
        private List<string> validationErrors = new List<string>();

        public List<string> ValidateManifest(PluginManifest manifest)
        {
            // TODO - validate manifest
            return validationErrors;
        }

        public void GenerateCustomisations(ConfigurationManifest manifest, IOrganizationService client, ILogger logger)
        {


        }
    }
}
