using CloudAwesome.Xrm.Core.Models;
using CloudAwesome.Xrm.Customisation.Cli.Features;
using CommandLine;

namespace CloudAwesome.Xrm.Customisation.Cli.CommandLineVerbs
{
    [Verb("roles", HelpText = "...")]
    public class ConfigureSecurityRoles: BaseCliVerb
    {
        public override void Run(string manifestPath, CdsConnection cdsConnection = null)
        {
            var generate = new RetrieveSolutionDependencies();
            generate.Run(manifestPath, cdsConnection);
        }
    }
}