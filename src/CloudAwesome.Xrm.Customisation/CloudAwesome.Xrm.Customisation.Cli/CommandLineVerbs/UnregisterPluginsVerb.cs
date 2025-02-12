using CloudAwesome.Xrm.Customisation.Cli.Features;
using CloudAwesome.Xrm.Customisation.Models;

namespace CloudAwesome.Xrm.Customisation.Cli.CommandLineVerbs
{
    public class UnregisterPluginsVerb: BaseCliVerb
    {
        public override void Run(string manifestPath, CdsConnection cdsConnection = null)
        {
            var unregister = new UnregisterPlugins();
            unregister.Run(manifestPath, cdsConnection);
        }
    }
}