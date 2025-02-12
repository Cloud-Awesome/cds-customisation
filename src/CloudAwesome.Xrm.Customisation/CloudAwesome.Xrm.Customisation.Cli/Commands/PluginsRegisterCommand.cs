using Spectre.Console;
using Spectre.Console.Cli;

namespace CloudAwesome.Xrm.Customisation.Cli.Commands
{
	public class PluginsRegisterCommand: Command<PluginRegisterCommandSettings>
	{
		public override int Execute(CommandContext context, PluginRegisterCommandSettings settings)
		{
			AnsiConsole.MarkupLine($"[green]Running 'placeholder' command with manifest:[/] ...");
			return 0;
		}
	}

	public class PluginRegisterCommandSettings: CommandSettings
	{
		public override ValidationResult Validate()
		{
			return base.Validate();
		}
	}
}