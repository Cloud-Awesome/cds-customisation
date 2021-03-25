using System.Collections.Generic;
using CloudAwesome.Xrm.Customisation.Cli.Features;
using CommandLine;

namespace CloudAwesome.Xrm.Customisation.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineActions>(args)
                .WithParsed(RunFeature);
        }

        public static void RunFeature(CommandLineActions options)
        {
            var features = new Dictionary<CommandLineActions.ActionOptions, IFeature>
            {
                { CommandLineActions.ActionOptions.GenerateCustomisations, new GenerateCustomisations() },
                { CommandLineActions.ActionOptions.RegisterPlugins, new RegisterPlugins() },
                { CommandLineActions.ActionOptions.UnregisterPlugins, new UnregisterPlugins() },
                { CommandLineActions.ActionOptions.ToggleProcesses, new ToggleProcesses() },
            };

            if (options.OverrideManifestConnectionDetails)
            {
                features[options.Action].Run(options.Manifest, options.CdsConnectionDetails);    
            }
            else
            {
                features[options.Action].Run(options.Manifest);
            }
            
        }
    }
}
