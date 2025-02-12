using System;
using System.Collections.Generic;
using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Customisation.ConfigurationManagement;
using CloudAwesome.Xrm.Customisation.DataverseExtensions;
using CloudAwesome.Xrm.Customisation.Exceptions;
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
            var configurationManifest = (ConfigurationManifest) manifest;
            var wrapper = new ConfigurationWrapper();

            ValidationResult = wrapper.Validate(configurationManifest);

            return ValidationResult;
        }

        public void Run(string manifestPath, CdsConnection cdsConnection)
        {
            // TODO - throw good errors if manifest can't be found/read/isInvalid
            var manifest = SerialisationWrapper.DeserialiseFromFile<ConfigurationManifest>(manifestPath);
            // TODO - throw good error if XrmClient is unauthorised/etc...
            var client = XrmClient.GetCrmServiceClientFromManifestConfiguration(manifest.CdsConnection);

            ValidateManifest(manifest);
            if (!ValidationResult.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Manifest is not valid. Not progressing with registration");
                Console.WriteLine($"\nErrors found in manifest validation:\n{ValidationResult}");
                Console.ResetColor();
                
                // QUESTION - Do you really want to throw an exception here, if you're already logging the val output?
                throw new InvalidManifestException("Exiting processing with exception - Manifest is not valid");
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
