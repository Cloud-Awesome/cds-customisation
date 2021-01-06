﻿using System.Collections.Generic;
using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Core.Models;
using Microsoft.Extensions.Logging;

namespace CloudAwesome.Xrm.Customisation.Cli.Features
{
    public class RegisterPlugins: IFeature
    {
        public string FeatureName => nameof(RegisterPlugins);
        public List<string> ValidationErrors { get; set; }
        
        public List<string> ValidateManifest(ICustomisationManifest manifest)
        {
            // TODO - validate manifest
            // TODO - move this back to the wrapper class, not in the CLI!
            ValidationErrors = new List<string>();
            return ValidationErrors;
        }

        public void Run(string manifestPath)
        {
            var manifest = SerialisationWrapper.DeserialiseFromFile<PluginManifest>(manifestPath);
            var client = XrmClient.GetCrmServiceClientFromManifestConfiguration(manifest.CdsConnection);

            ValidateManifest(manifest);
            if (ValidationErrors.Count > 0)
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

            var pluginWrapper = new PluginWrapper();
            pluginWrapper.RegisterPlugins(manifest, client);
        }
    }
}