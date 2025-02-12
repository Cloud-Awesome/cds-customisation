using CloudAwesome.Xrm.Customisation.Cli.Features;
using CloudAwesome.Xrm.Customisation.Models;
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