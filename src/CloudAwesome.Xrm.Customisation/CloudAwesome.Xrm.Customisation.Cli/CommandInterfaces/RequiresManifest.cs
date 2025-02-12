using System.ComponentModel;
using Spectre.Console.Cli;

namespace CloudAwesome.Xrm.Customisation.Cli.CommandInterfaces
{
	public abstract class RequiresManifest: CommandSettings
	{
		[CommandArgument(0, "<manifest>")]
		[Description("Path to the solution manifest file to import.")]
		public string Manifest { get; set; }
	}
}