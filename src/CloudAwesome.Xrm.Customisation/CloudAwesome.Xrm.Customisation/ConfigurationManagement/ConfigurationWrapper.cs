using System.Linq;
using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Customisation.EarlyBoundModels;
using CloudAwesome.Xrm.Customisation.Models;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Customisation.ConfigurationManagement
{
    public class ConfigurationWrapper
    {
        public ManifestValidationResult Validate(ConfigurationManifest manifest)
        {
            var validator = new ConfigurationManifestValidator();
            var result = validator.Validate(manifest);

            if (!result.IsValid)
            {
                return new ManifestValidationResult()
                {
                    IsValid = false,
                    Errors = result.Errors.Select(e => e.ErrorMessage)
                };
            }

            return new ManifestValidationResult()
            {
                IsValid = true
            };
        }
        
        /// <summary>
        /// Process ConfigurationManifest to generate Entity model, Security roles and Model driven apps
        /// </summary>
        /// <param name="manifest"></param>
        /// <param name="client"></param>
        public void GenerateCustomisations(ConfigurationManifest manifest, IOrganizationService client)
        {
            if (manifest.LoggingConfiguration != null)
            {
                var t = new TracingHelper(manifest.LoggingConfiguration);
                GenerateCustomisations(manifest, client, t);
            }
            else
            {
                GenerateCustomisations(manifest, client, t: null);
            }
        }

        /// <summary>
        /// Process ConfigurationManifest to generate Entity model, Security roles and Model driven apps
        /// Accepts custom ILogger implementation for custom logging output 
        /// </summary>
        /// <param name="manifest"></param>
        /// <param name="client"></param>
        /// <param name="logger"></param>
        public void GenerateCustomisations(ConfigurationManifest manifest, IOrganizationService client, ILogger logger)
        {
            var t = new TracingHelper(logger);
            GenerateCustomisations(manifest, client, t);
        }

        /// <summary>
        /// Process ConfigurationManifest to generate Entity model, Security roles and Model driven apps
        /// </summary>
        /// <param name="manifest"></param>
        /// <param name="client"></param>
        /// <param name="t"></param>
        public void GenerateCustomisations(ConfigurationManifest manifest, IOrganizationService client, TracingHelper t)
        {
            t.Debug($"Entering ConfigurationWrapper.GenerateCustomisations");
            t.Info($"Customisations with be generated in {manifest.SolutionName}");

            var publisherPrefix = GetPublisherPrefixFromSolution(client, manifest.SolutionName);
            t.Debug($"Publisher prefix retrieved: {publisherPrefix}");

            GlobalOptionSets.Generate(manifest, client, t, publisherPrefix);
            SecurityRoles.Generate(manifest, client, t, publisherPrefix);
            EntityModel.Generate(manifest, client, t, publisherPrefix);
            ModelDrivenApps.Generate(manifest, client, t, publisherPrefix);

            t.Info(">> Publishing all customisations");
            PublishAllCustomisations(client);
            
            t.Debug($"Exiting ConfigurationWrapper.GenerateCustomisations");
        }
        
        public static void PublishAllCustomisations(IOrganizationService client)
        {
            client.Execute(new PublishAllXmlRequest());
        }
        
        public string GetPublisherPrefixFromSolution(IOrganizationService client, string solutionName)
        {
            var fetchQuery = SolutionPublisherQuery.Replace("{{SolutionName}}", solutionName);
            var publisher = 
                QueryExtensions.RetrieveRecordFromQuery(client, new FetchExpression(fetchQuery))
                    .ToEntity<Publisher>();

            return publisher.CustomizationPrefix;
        }
        
        private const string SolutionPublisherQuery =
            @"<fetch version=""1.0"" output-format=""xml-platform"" mapping=""logical"" distinct=""true"">
	            <entity name=""publisher"">
		            <attribute name=""publisherid""/>
		            <attribute name=""friendlyname""/>
		            <attribute name=""uniquename""/>
		            <attribute name=""customizationprefix""/>
		            <link-entity name=""solution"" from=""publisherid"" to=""publisherid"" link-type=""inner"" alias=""p"">
			            <filter type=""and"">
				            <condition attribute=""uniquename"" operator=""eq"" value=""{{SolutionName}}""/>
			            </filter>
		            </link-entity>
	            </entity>
            </fetch>";
    }
}
