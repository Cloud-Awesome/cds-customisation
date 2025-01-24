using System.Text;
using CloudAwesome.Xrm.Customisation.Models;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation.SolutionDependencyCheck
{
	public class SolutionDependencies
	{
		public MissingComponent[] RetrieveSolutionDependencies(IOrganizationService client, byte[] solution)
		{
			var getMissingComponents = new RetrieveMissingComponentsRequest
			{
				CustomizationFile = solution
			};

			var missingComponents = (RetrieveMissingComponentsResponse)client.Execute(getMissingComponents);
			return missingComponents.MissingComponents;
		}

		public MissingDependenciesResult GetMissingSolutionDependencies(IOrganizationService client, byte[] solution)
		{
			var missingDependencies = RetrieveSolutionDependencies(client, solution);

			return new MissingDependenciesResult
			{
				MissingComponents = missingDependencies,
				MissingComponentsResult = FormatResultsAsString(missingDependencies)
			};
		}

		public string GetMissingSolutionDependenciesAsString(IOrganizationService client, byte[] solution)
		{
			var missingDependencies = RetrieveSolutionDependencies(client, solution);
			return FormatResultsAsString(missingDependencies);
		}

		private string FormatResultsAsString(MissingComponent[] missingComponents)
		{
			var result = new StringBuilder();
			result.Append("Solution dependency check result: \n");

			if (missingComponents.Length == 0)
			{
				result.Append("No missing dependencies found");
				return result.ToString();
			}
			
			foreach (var component in missingComponents)
			{
				result.Append(
					$"Dependent component: {component.RequiredComponent.DisplayName} ({component.RequiredComponent.SchemaName}) \n");
			}

			return result.ToString();
		}
	}
}