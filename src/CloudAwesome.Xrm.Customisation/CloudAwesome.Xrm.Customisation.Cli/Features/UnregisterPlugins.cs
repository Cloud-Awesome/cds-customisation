using System.Collections.Generic;
using CloudAwesome.Xrm.Customisation.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation.Cli.Features
{
    public class UnregisterPlugins: IFeature
    {
        public string FeatureName => nameof(UnregisterPlugins);
        public ManifestValidationResult ValidationResult { get; set; }

        public ManifestValidationResult ValidateManifest(ICustomisationManifest manifest)
        {
            ValidationResult = new ManifestValidationResult
            {
                IsValid = true
            };

            return ValidationResult;
        }

        public void Run(string manifestPath, CdsConnection cdsConnection)
        {
            throw new System.NotImplementedException();
        }
    }
}
