using System;
using CloudAwesome.Xrm.Customisation.Cli.Features;
using CloudAwesome.Xrm.Customisation.Models;

namespace CloudAwesome.Xrm.Customisation.Cli.CommandLineVerbs
{
    public class GenerateCustomisationsVerb: BaseCliVerb
    {
        public override void Run(string manifestPath, CdsConnection cdsConnection = null)
        {
            var generate = new GenerateCustomisations();
            generate.Run(manifestPath, cdsConnection);
        }
    }
}