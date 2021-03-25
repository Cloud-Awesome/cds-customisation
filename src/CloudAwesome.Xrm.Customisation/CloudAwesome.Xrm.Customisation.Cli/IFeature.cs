using System.Collections.Generic;
using CloudAwesome.Xrm.Core.Models;
using CloudAwesome.Xrm.Customisation.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation.Cli
{
    public interface IFeature
    {
        string FeatureName { get; }
        ManifestValidationResult ValidationResult { get; set; }
        
        ManifestValidationResult ValidateManifest(ICustomisationManifest manifest);

        void Run(string manifestPath, CdsConnection cdsConnection = null);

    }
}