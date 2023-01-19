using CloudAwesome.Xrm.Core.Models;
using CloudAwesome.Xrm.Customisation.Cli.Features;
using CommandLine;

namespace CloudAwesome.Xrm.Customisation.Cli.CommandLineVerbs
{
    [Verb("register", HelpText = "Register, unregister plugin assemblies, steps, custom APIs and service endpoints")]
    public class RegisterPluginsVerb: BaseCliVerb
    {
        public override void Run(string manifestPath, CdsConnection cdsConnection = null)
        {
            var registerPlugins = new RegisterPlugins();
            registerPlugins.Run(manifestPath, cdsConnection);
        }
        
        [Option("activate", HelpText = "Should the processes defined in the manifest be activated or deactivated. Overrides value in the manifest if set.")]
        public ProcessTargetStatusEnum TargetStatus { get; set; }
    }

    public enum ProcessTargetStatusEnum
    {
        Activate,
        Deactivate
    }
}