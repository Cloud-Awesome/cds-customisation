using System;
using CommandLine;

namespace CloudAwesome.Xrm.Customisation.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Parser.Default.ParseArguments<CommandLineActions>(args)
                .WithParsed<CommandLineActions>(Run);
        }

        public static void Run(CommandLineActions options)
        {
            switch (options.Action)
            {
                case CommandLineActions.ActionOptions.RegisterPlugins:
                    Console.WriteLine("I'm registering plugins now!!");
                    break;

                case CommandLineActions.ActionOptions.GenerateCustomisations:
                    Console.WriteLine("I'm generating entities and stuff now!!");
                    Console.WriteLine("Only joking, this action hasn't been implemented yet. Soz");
                    break;

                case CommandLineActions.ActionOptions.MigrateDeletionJobs:
                    Console.WriteLine("I'm migrating Bulk Deletion Jobs now!!");
                    Console.WriteLine("Only joking, this action hasn't been implemented yet. Soz");
                    break;

                case CommandLineActions.ActionOptions.ToggleProcesses:
                    Console.WriteLine("I'm either switching off a load of processes now, or I'm switching them back on...");
                    Console.WriteLine("Only joking, this action hasn't been implemented yet. Soz");
                    break;
            }
        }

        public static void Run()
        {

        }
    }
}
