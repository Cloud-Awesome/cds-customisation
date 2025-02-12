using CloudAwesome.Xrm.Customisation.Cli.Features;
using CloudAwesome.Xrm.Customisation.Models;

namespace CloudAwesome.Xrm.Customisation.Cli.CommandLineVerbs
{
    public class ToggleProcessesVerb: BaseCliVerb
    {
        public override void Run(string manifestPath, CdsConnection cdsConnection = null)
        {
            var toggle = new ToggleProcesses();
            toggle.Run(manifestPath, cdsConnection);
        }
    }
}