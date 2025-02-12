using CloudAwesome.Xrm.Customisation.Cli.Commands;
using CloudAwesome.Xrm.Customisation.Cli.Features;
using Spectre.Console.Cli;

namespace CloudAwesome.Xrm.Customisation.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var cli = new CommandApp();
            
            cli.Configure(config =>
            {
                config.SetApplicationName("dvcli");
                config.AddBranch("customisations", customisations =>
                {
                    customisations.SetDescription("Generate, test and tear down solution-aware artefacts rapidly (e.g. during a prototyping phases).");
                    
                    customisations.AddCommand<PlaceholderCommand>("import")
                        .WithDescription("Import customisations from manifest");
                    customisations.AddCommand<PlaceholderCommand>("clobber")
                        .WithDescription("Delete customisations defined in the manifest");
                    customisations.AddCommand<PlaceholderCommand>("generate-manifest")
                        .WithDescription("Generate a new baseline manifest based on a solution which can then be manually extended");
                });
                
                config.AddBranch("plugins", plugins =>
                {
                    plugins.SetDescription("Register plugin assemblies, steps, images, service endpoints and apis.");
                    
                    plugins.AddCommand<PluginsRegisterCommand>("register");
                    plugins.AddCommand<PlaceholderCommand>("activate");
                    plugins.AddCommand<PlaceholderCommand>("deactivate");
                    plugins.AddCommand<PlaceholderCommand>("clobber");
                    plugins.AddCommand<PlaceholderCommand>("generate-manifest");
                });
                
                config.AddBranch("dependencies", dependencies =>
                {
                    dependencies.SetDescription("Quickly verify any missing dependencies of an exported solution before importing.");
                    
                    dependencies.AddCommand<PlaceholderCommand>("check");
                });
                
                config.AddBranch("processes", processes =>
                {
                    processes.SetDescription("Activate/Deactivate various types of system processes (e.g. for data migration, imports, updates.");
                    
                    processes.AddCommand<PlaceholderCommand>("activate");
                    processes.AddCommand<PlaceholderCommand>("deactivate");
                });
                
                config.AddBranch("security", security =>
                {
                    security.SetDescription("Export/Import team security role assignments.");
                    
                    security.AddCommand<PlaceholderCommand>("import-team-roles");
                    security.AddCommand<PlaceholderCommand>("export-team-roles");
                });
                
                config.AddBranch("document", document =>
                {
                    document.SetDescription("Generate markdown documentation for defined areas of the system.");
                    
                    document.AddCommand<PlaceholderCommand>("customisations");
                    document.AddCommand<PlaceholderCommand>("plugins");
                    document.AddCommand<PlaceholderCommand>("security");
                });

                config.AddBranch("project-ops", pjops =>
                {
                    pjops.SetDescription("Quickly configure entity and optionset-based pricing dimensions");

                    pjops.AddCommand<PlaceholderCommand>("generate-pricing-dimensions");
                });
            });

            cli.Run(args);
        }
    }
}
