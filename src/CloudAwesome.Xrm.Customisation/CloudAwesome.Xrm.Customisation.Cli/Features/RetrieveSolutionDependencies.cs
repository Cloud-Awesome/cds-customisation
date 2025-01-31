using System;
using System.IO;
using System.Net;
using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Core.Models;
using CloudAwesome.Xrm.Customisation.Exceptions;
using CloudAwesome.Xrm.Customisation.Models;
using CloudAwesome.Xrm.Customisation.PluginRegistration;
using CloudAwesome.Xrm.Customisation.SolutionDependencyCheck;
using Microsoft.Extensions.Logging;

namespace CloudAwesome.Xrm.Customisation.Cli.Features
{
    public class RetrieveSolutionDependencies: IFeature
    {
        public string FeatureName => nameof(RetrieveSolutionDependencies);
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
            var manifest = SerialisationWrapper.DeserialiseJsonFromFile<SolutionDependencyCheckManifest>(manifestPath);
            
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

            var solution = File.ReadAllBytes(manifest.SolutionFilepath);
            
            var tester = new SolutionDependencies();
            var dependencies = tester.GetMissingSolutionDependencies(client, solution);
            
            Console.WriteLine(dependencies.MissingComponentsResult);
        }
    }
}
