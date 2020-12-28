using System;
using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Core.Loggers;
using CloudAwesome.Xrm.Core.Models;
using CommandLine;
using Microsoft.Extensions.Logging;

namespace CloudAwesome.Xrm.Customisation.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineActions>(args).WithParsed(Run);
        }

        public static void Run(CommandLineActions options)
        {
            switch (options.Action)
            {
                case CommandLineActions.ActionOptions.RegisterPlugins:
                    Console.WriteLine("I'm registering plugins now!!");

                    // TODO - Verify the manifest against XSD before continuing #5

                    var manifest = SerialisationWrapper.DeserialiseFromFile<PluginManifest>(options.Manifest);
                    var client = XrmClient.GetCrmServiceClientFromManifestConfiguration(manifest.CdsConnection);
                    
                    // Temp until LoggingConfiguration is resolved in .Core
                    var consoleLogger = new ConsoleLogger(LogLevel.Debug);

                    var pluginWrapper = new PluginWrapper();
                    var validationOutput = pluginWrapper.ValidateManifest(manifest);

                    if (validationOutput.Count > 0)
                    {
                        //  TODO - Output the errors

                        return;
                    }

                    pluginWrapper.RegisterPlugins(manifest, client, consoleLogger);
                    pluginWrapper.RegisterServiceEndpoints(manifest, client, consoleLogger);

                    Console.WriteLine("Plugin registration completed! Woop! =D");
                    break;

                case CommandLineActions.ActionOptions.UnregisterPlugins:
                    Console.WriteLine("I'm unregistering plugins now!!");

                    var manifest2 = SerialisationWrapper.DeserialiseFromFile<PluginManifest>(options.Manifest);
                    var client2 = XrmClient.GetCrmServiceClientFromManifestConfiguration(manifest2.CdsConnection);

                    if (manifest2.LoggingConfiguration == null)
                    {
                        manifest2.LoggingConfiguration = new LoggingConfiguration()
                        {
                            LoggerConfigurationType = LoggerConfigurationType.Console,
                            LogLevelToTrace = LogLevel.Information
                        };
                    }

                    var pluginWrapper2 = new PluginWrapper();
                    pluginWrapper2.UnregisterPlugins(manifest2, client2);

                    Console.WriteLine("Plugin de-registration completed! Aaaggh! =D");
                    break;

                case CommandLineActions.ActionOptions.GenerateCustomisations:
                    Console.WriteLine("I'm generating entities and stuff now!!");

                    var manifest3 = SerialisationWrapper.DeserialiseFromFile<ConfigurationManifest>(options.Manifest);
                    var client3 = XrmClient.GetCrmServiceClientFromManifestConfiguration(manifest3.CdsConnection);

                    // Temp until LoggingConfiguration is resolved in .Core
                    var consoleLogger2 = new ConsoleLogger(LogLevel.Debug);

                    if (manifest3.LoggingConfiguration == null)
                    {
                        manifest3.LoggingConfiguration = new LoggingConfiguration()
                        {
                            LoggerConfigurationType = LoggerConfigurationType.Console,
                            LogLevelToTrace = LogLevel.Information
                        };
                    }

                    var configurationWrapper = new ConfigurationWrapper();
                    configurationWrapper.GenerateCustomisations(manifest3, client3, consoleLogger2);

                    Console.WriteLine("Only joking, this action hasn't been implemented yet. Soz");
                    break;

                case CommandLineActions.ActionOptions.MigrateDeletionJobs:
                    Console.WriteLine("I'm migrating Bulk Deletion Jobs now!!");
                    Console.WriteLine("Only joking, this action hasn't been implemented yet. Soz");
                    break;

                case CommandLineActions.ActionOptions.ToggleProcesses:
                    Console.WriteLine("I'm either switching off a load of processes now, or I'm switching them back on...");

                    var manifest4 =
                        SerialisationWrapper.DeserialiseFromFile<ProcessActivationManifest>(options.Manifest);
                    var client4 = XrmClient.GetCrmServiceClientFromManifestConfiguration(manifest4.CdsConnection);

                    // Temp until LoggingConfiguration is resolved in .Core
                    var consoleLogger4 = new ConsoleLogger(LogLevel.Debug);

                    if (manifest4.LoggingConfiguration == null)
                    {
                        manifest4.LoggingConfiguration = new LoggingConfiguration()
                        {
                            LoggerConfigurationType = LoggerConfigurationType.Console,
                            LogLevelToTrace = LogLevel.Information
                        };
                    }

                    var processActivationWrapper = new ProcessActivationWrapper();
                    processActivationWrapper.SetStatusFromManifest(client4, manifest4, consoleLogger4);

                    Console.WriteLine("Only joking, this action hasn't been implemented yet. Soz");
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
