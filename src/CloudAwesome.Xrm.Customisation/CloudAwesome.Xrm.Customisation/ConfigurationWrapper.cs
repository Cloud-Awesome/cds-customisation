﻿using System.Collections.Generic;
using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Customisation.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Customisation
{
    public class ConfigurationWrapper
    {
        private List<string> validationErrors = new List<string>();

        public List<string> ValidateManifest(PluginManifest manifest)
        {
            // TODO - validate manifest
            return validationErrors;
        }

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

        public void GenerateCustomisations(ConfigurationManifest manifest, IOrganizationService client, ILogger logger)
        {
            var t = new TracingHelper(logger);
            GenerateCustomisations(manifest, client, t);
        }

        public void GenerateCustomisations(ConfigurationManifest manifest, IOrganizationService client, TracingHelper t)
        {
            t.Debug($"Entering ConfigurationWrapper.GenerateCustomisations");

            var publisherPrefix = GetPublisherPrefixFromSolution(client, manifest.SolutionName);
            t.Debug($"Publisher prefix retrieved: {publisherPrefix}");

            foreach (var entity in manifest.Entities)
            {
                t.Debug($"Processing entity: {entity.DisplayName}");
                entity.CreateOrUpdate(client, publisherPrefix, manifest);
                t.Info($"Entity {entity.DisplayName} created or updated");

                if (entity.Attributes == null) continue;
                foreach (var attribute in entity.Attributes)
                {
                    t.Debug($"Processing attribute: {attribute.DisplayName}");
                    attribute.EntitySchemaName = entity.SchemaName;

                    try
                    {
                        attribute.CreateOrUpdate(client, publisherPrefix, manifest);
                        t.Info($"Attribute {attribute.DisplayName} successfully processed");
                    }
                    catch (NotCustomisableException e)
                    {
                        t.Warning(e.Message);
                    }
                    
                    if (attribute.AddToForm) attribute.AddToSystemForms();
                    t.Debug($"Attribute {attribute.DisplayName} added to form");

                    if (attribute.AddToViewOrder.HasValue) attribute.AddToSystemViews();
                    t.Debug($"Attribute {attribute.DisplayName} added to views");

                    t.Info($"Attribute {attribute.DisplayName} successfully processed");
                }

                t.Info($"Entity {entity.DisplayName} successfully processed");
            }

            t.Debug($"Exiting ConfigurationWrapper.GenerateCustomisations");
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
