using System;
using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Customisation;
using CloudAwesome.Xrm.Customisation.Models;
using CommandLine;
using Microsoft.Xrm.Sdk;

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

                    var manifest = SerialisationWrapper.DeserialiseFromFile<PluginManifest>(options.Manifest);
                    var client = GetClientFromManifestConfiguration(manifest.CdsConnection);

                    var pluginWrapper = new PluginWrapper();
                    pluginWrapper.RegisterPlugins(manifest, client);

                    Console.WriteLine("Plugin registration completed! Woop! =D");
                    break;

                case CommandLineActions.ActionOptions.GenerateCustomisations:
                    Console.WriteLine("I'm generating entities and stuff now!!");
                    Console.WriteLine("Only joking, this action hasn't been implemented yet. Soz");
                    break;

                case CommandLineActions.ActionOptions.MigrateDeletionJobs:
                    Console.WriteLine("I'm migrating Bulk Deletion Jobs now!!");
                    Console.WriteLine("Only joking, this action hasn't been implemented yet. Soz");
                    break;

                case CommandLineActions.ActionOptions.ToggleProcesses:
                    Console.WriteLine("I'm either switching off a load of processes now, or I'm switching them back on...");
                    Console.WriteLine("Only joking, this action hasn't been implemented yet. Soz");
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static IOrganizationService GetClientFromManifestConfiguration(CdsConnection cdsConnection)
        {
            // TODO - Scope this out to the Core solution
            switch (cdsConnection.ConnectionType)
            {
                case CdsConnectionType.AppRegistration:
                    return XrmClient.GetCrmServiceClientWithClientSecret(cdsConnection.CdsUrl, cdsConnection.CdsAppId,
                        cdsConnection.CdsAppSecret);
                case CdsConnectionType.ConnectionString:
                    return XrmClient.GetCrmServiceClient(cdsConnection.CdsConnectionString);
                case CdsConnectionType.UserNameAndPassword:
                    return XrmClient.GetCrmServiceClientWithO365(cdsConnection.CdsUrl, cdsConnection.CdsUserName,
                        cdsConnection.CdsPassword);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
