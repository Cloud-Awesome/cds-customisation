using System;
using CloudAwesome.Xrm.Core.Models;
using CloudAwesome.Xrm.Customisation.Cli.Features;
using CommandLine;

namespace CloudAwesome.Xrm.Customisation.Cli.CommandLineVerbs
{
    [Verb("generate", HelpText = "Generate customisations such as entity model, security roles, and model driven apps")]
    public class GenerateCustomisationsVerb: BaseCliVerb
    {
        public override void Run(string manifestPath, CdsConnection cdsConnection = null)
        {
            var generate = new GenerateCustomisations();
            generate.Run(manifestPath, cdsConnection);
        }
    }
}