﻿using CommandLine;

namespace CloudAwesome.Xrm.Customisation.Cli
{
    [Verb("RegisterPlugins", HelpText = "Register plugin assemblies, steps and images based detailed in XML manifest")]
    public class CommandLineActions
    {
        public enum ActionOptions
        {
            RegisterPlugins = 0,
            GenerateCustomisations = 1,
            MigrateDeletionJobs = 2,
            ToggleProcesses = 3
        }

        [Option('a', "action", Required = true)]
        public ActionOptions Action { get; set; }

        [Option('m', "manifest", Required = true)]
        public string Manifest { get; set; }

        // TODO - set this as an enum later...
        [Option("auth", Required = true)]
        public string AuthenticationType { get; set; }

    }
}