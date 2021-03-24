using System.Collections.Generic;
using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Core.Models;
using CloudAwesome.Xrm.Customisation.Models;
using Microsoft.Extensions.Logging;

namespace CloudAwesome.Xrm.Customisation.Cli.Features
{
    public class GenerateCustomisations: IFeature
    {
        public string FeatureName => nameof(GenerateCustomisations);
        public ManifestValidationResult ValidationResult { get; set; }

        public ManifestValidationResult ValidateManifest(ICustomisationManifest manifest)
        {
            ValidationResult = new ManifestValidationResult
            {
                IsValid = true
            };

            return ValidationResult;
        }

        public void Run(string manifestPath)
        {
            var manifest = SerialisationWrapper.DeserialiseFromFile<ConfigurationManifest>(manifestPath);
            var client = XrmClient.GetCrmServiceClientFromManifestConfiguration(manifest.CdsConnection);

            ValidateManifest(manifest);
            if (!ValidationResult.IsValid)
            {
                // TODO - throw a good error ;)
                return;
            }

            if (manifest.LoggingConfiguration == null)
            {
                manifest.LoggingConfiguration = new LoggingConfiguration()
                {
                    LoggerConfigurationType = LoggerConfigurationType.Console,
                    LogLevelToTrace = LogLevel.Information
                };
            }

            var configWrapper = new ConfigurationWrapper();
            configWrapper.GenerateCustomisations(manifest, client);
        }
    }
}
