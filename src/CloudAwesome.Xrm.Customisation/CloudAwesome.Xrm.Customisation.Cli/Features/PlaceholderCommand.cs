using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace CloudAwesome.Xrm.Customisation.Cli.Features
{
	public class PlaceholderCommand: Command<PlaceholderSettings>
	{
		public override int Execute(CommandContext context, PlaceholderSettings settings)
		{
			var manifestPath = settings.ManifestPath;
			AnsiConsole.MarkupLine($"[green]Running 'placeholder' command with manifest:[/] {manifestPath}");
			return 0;
		}
	}
	
	public class PlaceholderSettings : CommandSettings
	{
		[CommandArgument(0, "<manifest>")]
		[Description("Path to the solution manifest file to import.")]
		public string ManifestPath { get; set; }

		[CommandArgument(1, "[force]")]
		[Description("Force the import even if there are conflicts.")]
		[DefaultValue(false)]
		public bool Force { get; set; }

		/*public override ValidationResult Validate()
		{
			return string.IsNullOrEmpty(ManifestPath)
				? ValidationResult.Error("--manifest is a mandatory input")
				: ValidationResult.Success();
		}*/
	}
}