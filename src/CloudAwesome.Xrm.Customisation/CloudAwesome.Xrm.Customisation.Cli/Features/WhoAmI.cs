using System;
using System.ComponentModel;
using CloudAwesome.Xrm.Customisation.Models;
using Spectre.Console.Cli;

namespace CloudAwesome.Xrm.Customisation.Cli.Features;

public class WhoAmI: Command<WhoAmISettings>
{
	public override int Execute(CommandContext context, WhoAmISettings settings)
	{
		Console.WriteLine("You're Arthur...");
		return 0;
	}
}

public class WhoAmISettings : CommandSettings
{
	[CommandArgument(0, "<url>")]
	[Description("URL of the target dataverse environment")]
	public string Url { get; set; }

	[CommandArgument(1, "[bearer-token]")]
	[Description("Previously generated bearer token authenticating you to the target environment")]
	public string BearerToken { get; set; }
}