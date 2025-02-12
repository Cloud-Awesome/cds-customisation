using CloudAwesome.Xrm.Customisation.Models;

namespace CloudAwesome.Xrm.Customisation.Cli.Features;

public class WhoAmI: IFeature
{
	public string FeatureName => "WhoAmI";
	public ManifestValidationResult ValidationResult { get; set; }
	public ManifestValidationResult ValidateManifest(ICustomisationManifest manifest)
	{
		throw new System.NotImplementedException();
	}

	public void Run(string manifestPath, CdsConnection cdsConnection = null)
	{
		
		
		
	}
}