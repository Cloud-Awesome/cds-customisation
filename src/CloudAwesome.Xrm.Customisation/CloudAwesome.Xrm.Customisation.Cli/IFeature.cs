using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation.Cli
{
    public interface IFeature
    {
        string FeatureName { get; }
        List<string> ValidationErrors { get; set; }
        
        List<string> ValidateManifest(ICustomisationManifest manifest);

        void Run(string manifestPath);

    }
}