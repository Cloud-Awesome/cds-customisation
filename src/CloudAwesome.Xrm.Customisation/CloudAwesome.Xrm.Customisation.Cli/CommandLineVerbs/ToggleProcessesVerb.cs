using System;
using CloudAwesome.Xrm.Core.Models;
using CloudAwesome.Xrm.Customisation.Cli.Features;
using CommandLine;

namespace CloudAwesome.Xrm.Customisation.Cli.CommandLineVerbs
{
    [Verb("toggle", HelpText = "Toggle status of system processes such as plugin steps, flows, and workflows")]
    public class ToggleProcessesVerb: BaseCliVerb
    {
        public override void Run(string manifestPath, CdsConnection cdsConnection = null)
        {
            var toggle = new ToggleProcesses();
            toggle.Run(manifestPath, cdsConnection);
        }
    }
}