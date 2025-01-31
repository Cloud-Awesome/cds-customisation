using System.Collections.Generic;
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
					$"Missing component: \"{component.RequiredComponent.DisplayName}\" ({component.RequiredComponent.SchemaName}) of type '{GetSolutionType(component.RequiredComponent.Type)}' " +
					$"is required by \"{component.DependentComponent.DisplayName}\" ({GetSolutionType(component.DependentComponent.Type)})");
			}

			return result.ToString();
		}

		private string GetSolutionType(int solutionType)
		{
			return _solutionType[solutionType];
		}
		
		private readonly Dictionary<int, string> _solutionType =
            new Dictionary<int, string>()
            {
                { 1, "Entity" },
                { 2, "Field" },
                { 3, "Relationship" },
                { 4, "Attribute Picklist Value" },
                { 5, "Attribute Lookup Value" },
                { 6, "View Attribute" },
                { 7, "Localized Label" },
                { 8, "Relationship Extra Condition" },
                { 9, "Option Set" },
                { 10, "Entity Relationship" },
                { 11, "Entity Relationship Role" },
                { 12, "Entity Relationship Relationships" },
                { 13, "Managed Property" },
                { 14, "Entity Key" },
                { 16, "Privilege" },
                { 17, "PrivilegeObjectTypeCode" },
                { 18, "Index" },
                { 20, "Role" },
                { 21, "Role Privilege" },
                { 22, "Display String" },
                { 23, "Display String Map" },
                { 24, "Form" },
                { 25, "Organization" },
                { 26, "View" },
                { 29, "Dialog/Workflow" },
                { 31, "Report" },
                { 32, "Report Entity" },
                { 33, "Report Category" },
                { 34, "Report Visibility" },
                { 35, "Attachment" },
                { 36, "Email Template" },
                { 37, "Contract Template" },
                { 38, "KB Article Template" },
                { 39, "Mail Merge Template" },
                { 44, "Duplicate Rule" },
                { 45, "Duplicate Rule Condition" },
                { 46, "Entity Map" },
                { 47, "Attribute Map" },
                { 48, "Ribbon Command" },
                { 49, "Ribbon Context Group" },
                { 50, "Ribbon Customization" },
                { 52, "Ribbon Rule" },
                { 53, "Ribbon Tab To Command Map" },
                { 55, "Ribbon Diff" },
                { 59, "Chart" },
                { 60, "View" },
                { 61, "Web Resource" },
                { 62, "Site Map" },
                { 63, "Connection Role" },
                { 64, "Complex Control" },
                { 65, "Hierachy Rule" },
                { 66, "Custom Control" },
                { 68, "Custom Control Default Config" },
                { 70, "Field Security Profile" },
                { 71, "Field Permission" },
                { 90, "Plugin Type"},
                { 91, "Plugin Assembly" },
                { 92, "SDK Message Processing Step" },
                { 93, "SDK Message Processing Step Image" },
                { 95, "Service Endpoint" },
                { 150, "Routing Rule" },
                { 151, "Routing Rule Item" },
                { 152, "SLA" },
                { 153, "SLA Item" },
                { 154, "Convert Rule"},
                { 155, "Convert Rule Item" },
                { 161, "Mobile Offline Profile" },
                { 162, "Mobile Offline Profile Item" },
                { 165, "Similarity Rule" },
                { 166, "Data Source Mapping"},
                { 201, "SDKMessage" },
                { 202, "SDKMessageFilter" },
                { 203, "SdkMessagePair" },
                { 204, "SdkMessageRequest" },
                { 205, "SdkMessageRequestField" },
                { 206, "SdkMessageResponse" },
                { 207, "SdkMessageResponseField" },
                { 208, "Import Map" },
                { 210, "WebWizard" },
                { 300, "Canvas App" },
                { 371, "Connector" },
                { 372, "Connector" },
                { 380, "Environment Variable Definition" },
                { 381, "Environment Variable Value" },
                { 400, "AI Project Type" },
                { 401, "AI Project" },
                { 402, "AI Configuration" },
                { 430, "Entity Analytics Configuration" },
                { 431, "Attribute Image Configuration" },
                { 432, "Entity Image Configuration" }

            };
	}
}