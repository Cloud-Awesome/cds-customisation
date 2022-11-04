using System;
using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Core.Models;
using CloudAwesome.Xrm.Customisation.Exceptions;
using CloudAwesome.Xrm.Customisation.Models;
using CloudAwesome.Xrm.Customisation.PluginRegistration;
using Microsoft.Extensions.Logging;

namespace CloudAwesome.Xrm.Customisation.Cli.Features
{
    public class RegisterPlugins: IFeature
    {
        public string FeatureName => nameof(RegisterPlugins);
        public ManifestValidationResult ValidationResult { get; set; }

        public ManifestValidationResult ValidateManifest(ICustomisationManifest manifest)
        {
            var pluginManifest = (PluginManifest) manifest;
            var pluginWrapper = new PluginWrapper();

            ValidationResult = PluginWrapper.Validate(pluginManifest);
            
            return ValidationResult;
        }

        public void Run(string manifestPath, CdsConnection cdsConnection)
        {
            var manifest = SerialisationWrapper.DeserialiseFromFile<PluginManifest>(manifestPath);
            
            if (cdsConnection != null)
            {
                manifest.CdsConnection = cdsConnection;
            }
            
            ValidateManifest(manifest);
            if (!ValidationResult.IsValid)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Manifest is not valid. Not progressing with registration");
                Console.WriteLine($"\nErrors found in manifest validation:\n{ValidationResult}");
                Console.ResetColor();
                
                throw new InvalidManifestException("Exiting processing with exception - Manifest is no valid");
            }

            if (manifest.LoggingConfiguration == null)
            {
                manifest.LoggingConfiguration = new LoggingConfiguration()
                {
                    LoggerConfigurationType = LoggerConfigurationType.Console,
                    LogLevelToTrace = LogLevel.Information
                };
            }

            var client = XrmClient.GetCrmServiceClientFromManifestConfiguration(manifest.CdsConnection);
            
            
            var pluginWrapper = new PluginWrapper();
            pluginWrapper.RegisterPlugins(manifest, client);
            pluginWrapper.RegisterServiceEndpoints(manifest, client);
        }
    }
}
