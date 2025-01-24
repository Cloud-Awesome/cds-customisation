using Microsoft.Crm.Sdk.Messages;

namespace CloudAwesome.Xrm.Customisation.Models
{
	public class MissingDependenciesResult
	{
		public MissingComponent[] MissingComponents { get; set; }
		
		public string MissingComponentsResult { get; set; }

		public int NumberOfMissingResults => MissingComponents.Length;
	}
}