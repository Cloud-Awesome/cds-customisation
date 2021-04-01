using System;
using CloudAwesome.Xrm.Core.Models;
using CloudAwesome.Xrm.Customisation.Cli.Features;
using CommandLine;

namespace CloudAwesome.Xrm.Customisation.Cli.CommandLineVerbs
{
    [Verb("unregister", HelpText = "Register, unregister plugin assemblies, steps, custom APIs and service endpoints")]
    public class UnregisterPluginsVerb: BaseCliVerb
    {
        public override void Run(string manifestPath, CdsConnection cdsConnection = null)
        {
            var unregister = new UnregisterPlugins();
            unregister.Run(manifestPath, cdsConnection);
        }
    }
}