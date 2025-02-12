using System.ComponentModel;
using Spectre.Console.Cli;

namespace CloudAwesome.Xrm.Customisation.Cli.CommandInterfaces
{
	public abstract class AcceptsManifest: CommandSettings
	{
		[CommandOption("-m|--manifest")]
		[Description("File path to the json manifest")]
		public string Manifest { get; set; }
	}
}