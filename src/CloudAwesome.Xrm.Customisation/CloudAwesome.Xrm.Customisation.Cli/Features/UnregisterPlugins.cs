using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation.Cli.Features
{
    public class UnregisterPlugins: IFeature
    {
        public string FeatureName => nameof(UnregisterPlugins);
        public List<string> ValidationErrors { get; set; }

        public List<string> ValidateManifest(ICustomisationManifest manifest)
        {
            // TODO - move this back to the wrapper class, not in the CLI!
            throw new System.NotImplementedException();
        }

        public void Run(string manifestPath)
        {
            throw new System.NotImplementedException();
        }
    }
}
