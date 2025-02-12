using CloudAwesome.Xrm.Customisation.Cli.Features;
using CloudAwesome.Xrm.Customisation.Models;
using CommandLine;

namespace CloudAwesome.Xrm.Customisation.Cli.CommandLineVerbs
{
    [Verb("dependencies", HelpText = "Retrieve dependencies for an exported solution against a target dataverse environment")]
    public class RetrieveSolutionDependenciesVerb: BaseCliVerb
    {
        public override void Run(string manifestPath, CdsConnection cdsConnection = null)
        {
            var generate = new RetrieveSolutionDependencies();
            generate.Run(manifestPath, cdsConnection);
        }
    }
}