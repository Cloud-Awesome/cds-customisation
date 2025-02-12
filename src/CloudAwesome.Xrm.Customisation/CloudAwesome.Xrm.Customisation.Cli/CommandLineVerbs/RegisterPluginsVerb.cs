using CloudAwesome.Xrm.Customisation.Cli.Features;
using CloudAwesome.Xrm.Customisation.Models;

namespace CloudAwesome.Xrm.Customisation.Cli.CommandLineVerbs
{
    public class RegisterPluginsVerb: BaseCliVerb
    {
        public override void Run(string manifestPath, CdsConnection cdsConnection = null)
        {
            var registerPlugins = new RegisterPlugins();
            registerPlugins.Run(manifestPath, cdsConnection);
        }
        
        public ProcessTargetStatusEnum TargetStatus { get; set; }
    }

    public enum ProcessTargetStatusEnum
    {
        Activate,
        Deactivate
    }
}