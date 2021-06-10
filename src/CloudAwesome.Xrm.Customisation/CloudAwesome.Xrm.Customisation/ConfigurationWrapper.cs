using System;
using System.Linq;
using System.ServiceModel;
using System.Xml;
using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Customisation.Exceptions;
using CloudAwesome.Xrm.Customisation.Models;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Customisation
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
            t.Debug($"Customisations with be generated in {manifest.SolutionName}");

            var publisherPrefix = GetPublisherPrefixFromSolution(client, manifest.SolutionName);
            t.Debug($"Publisher prefix retrieved: {publisherPrefix}");

            CreateGlobalOptionSets(manifest, client, t, publisherPrefix);
            CreateSecurityRoles(manifest, client, t, publisherPrefix);
            GenerateEntityModel(manifest, client, t, publisherPrefix);
            CreateModelDrivenApps(manifest, client, t, publisherPrefix);

            t.Info(">> Publishing all customisations");
            PublishAllCustomisations(client);
            
            t.Debug($"Exiting ConfigurationWrapper.GenerateCustomisations");
        }
        
        /// <summary>
        /// Create or update global option sets from ConfigurationManifest
        /// </summary>
        /// <param name="manifest"></param>
        /// <param name="client"></param>
        /// <param name="t"></param>
        /// <param name="publisherPrefix"></param>
        public void CreateGlobalOptionSets(ConfigurationManifest manifest, IOrganizationService client, TracingHelper t, string publisherPrefix)
        {
            if (manifest.OptionSets == null)
            {
                t.Debug($"No global optionsets in manifest to be processed");
            }
            else
            {
                foreach (var optionSet in manifest.OptionSets)
                {
                    t.Debug($"Processing global option set: {optionSet.DisplayName}");

                    var targetOptionSet = new OptionSetMetadata
                    {
                        Name = string.IsNullOrEmpty(optionSet.SchemaName) ? optionSet.DisplayName.GenerateLogicalNameFromDisplayName(publisherPrefix) : optionSet.SchemaName,
                        DisplayName = new Label(optionSet.DisplayName, 1033),
                        IsGlobal = true,
                        OptionSetType = OptionSetType.Picklist,
                    };
                    foreach (var option in optionSet.Items)
                    {
                        targetOptionSet.Options.Add(new OptionMetadata(option.CreateLabelFromString(), null));
                    }

                    bool existingOptionSet;
                    try
                    {
                        var retrieveOptionSet = (RetrieveOptionSetResponse) client.Execute(new RetrieveOptionSetRequest()
                        {
                            Name = optionSet.SchemaName
                        });
                        var retrievedMetadataAttribute = retrieveOptionSet.OptionSetMetadata;
                        targetOptionSet.DisplayName = optionSet.DisplayName == null
                            ? retrievedMetadataAttribute.DisplayName
                            : optionSet.DisplayName.CreateLabelFromString();
                        targetOptionSet.MetadataId = retrievedMetadataAttribute.MetadataId;

                        existingOptionSet = true;
                    }
                    catch (FaultException)
                    {
                        existingOptionSet = false;
                    }

                    if (existingOptionSet)
                    {
                        client.Execute(new UpdateOptionSetRequest()
                        {
                            MergeLabels = true,
                            OptionSet = targetOptionSet
                        });
                        if (targetOptionSet.MetadataId != null)
                        {
                            SolutionWrapper.AddSolutionComponent(client, manifest.SolutionName, targetOptionSet.MetadataId.Value, ComponentType.OptionSet);
                        }
                    }
                    else
                    {
                        var response = (CreateOptionSetResponse) client.Execute(new CreateOptionSetRequest()
                        {
                            OptionSet = targetOptionSet
                        });
                        SolutionWrapper.AddSolutionComponent(client, manifest.SolutionName, response.OptionSetId, ComponentType.OptionSet);
                    }
                    
                    t.Info($"Successfully processed global option set: {optionSet.DisplayName}");
                }
            }
        }

        /// <summary>
        /// Create or update security roles
        /// </summary>
        /// <param name="manifest"></param>
        /// <param name="client"></param>
        /// <param name="t"></param>
        /// <param name="publisherPrefix"></param>
        public void CreateSecurityRoles(ConfigurationManifest manifest, IOrganizationService client, TracingHelper t, string publisherPrefix)
        {
            if (manifest.SecurityRoles == null)
            {
                t.Debug($"No security roles in manifest to be processed");
            }
            else
            {
                var rootBusinessUnit = XrmClient.GetRootBusinessUnit(client);

                foreach (var securityRole in manifest.SecurityRoles)
                {
                    var existingRole = GetExistingSecurityRoleQuery(securityRole.Name, rootBusinessUnit)
                        .RetrieveSingleRecord(client);

                    var role = new Role()
                    {
                        Name = securityRole.Name,
                        BusinessUnitId = rootBusinessUnit
                    }.CreateOrUpdate(client, GetExistingSecurityRoleQuery(securityRole.Name, rootBusinessUnit));
                    
                    SolutionWrapper.AddSolutionComponent(client, manifest.SolutionName, role.Id, ComponentType.Role);

                    if (securityRole.Privileges == null) continue;
                    foreach (var rolePrivilege in securityRole.Privileges)
                    {
                        var retrievePrivilegesByName = new QueryExpression("privilege")
                        {
                            ColumnSet = new ColumnSet(true),
                            Criteria = new FilterExpression
                            {
                                Conditions =
                                {
                                    new ConditionExpression("name", ConditionOperator.Equal, rolePrivilege)
                                }
                            }
                        };
                        var privilegeId = retrievePrivilegesByName.RetrieveSingleRecord(client);
                        if (privilegeId == null)
                        {
                            t.Warning($"Privilege with name {rolePrivilege} cannot be found. " +
                                      $"{rolePrivilege} for role {securityRole.Name} has been skipped");
                            continue;
                        }

                        var addedPrivilege = new AddPrivilegesRoleRequest()
                        {
                            RoleId = role.Id,
                            Privileges = new []
                            {
                                new RolePrivilege()
                                {
                                    PrivilegeId = privilegeId.Id,
                                    Depth = PrivilegeDepth.Global
                                }
                            }
                        };
                        client.Execute(addedPrivilege);
                    }
                }
            }
        }

        /// <summary>
        /// Create entities and attributes from a ConfigurationManifest
        /// </summary>
        /// <param name="manifest"></param>
        /// <param name="client"></param>
        /// <param name="t"></param>
        /// <param name="publisherPrefix"></param>
        public void GenerateEntityModel(ConfigurationManifest manifest, IOrganizationService client, TracingHelper t, string publisherPrefix)
        {
            if (manifest.Entities == null)
            {
                t.Debug($"No entities in manifest to be processed");
            }
            else
            {
                foreach (var entity in manifest.Entities)
                {
                    t.Debug($"Processing entity: {entity.DisplayName}");
                    entity.CreateOrUpdate(client, publisherPrefix, manifest);
                    t.Info($"Entity {entity.DisplayName} created or updated");

                    if (entity.Attributes == null)
                    {
                        t.Debug($"No attributes to process for entity {entity.DisplayName}");
                    }
                    else
                    {
                        XmlDocument formXml = null;
                        
                        foreach (var attribute in entity.Attributes)
                        {
                            t.Debug($"Processing attribute: {attribute.DisplayName}");
                            attribute.EntitySchemaName = entity.SchemaName;

                            try
                            {
                                var attributeMetaData = attribute.CreateOrUpdate(client, publisherPrefix, manifest);
                                
                                if (attribute.AddToForm) FormHelper.AddAttributeToForm(attributeMetaData, ref formXml);
                                t.Debug($"Attribute {attribute.DisplayName} added to form");
                                
                                // TODO - add to views
                                if (attribute.AddToViewOrder.HasValue) attribute.AddToSystemViews();
                                t.Debug($"Attribute {attribute.DisplayName} added to views");
                                
                                t.Info($"Attribute {attribute.DisplayName} successfully processed");
                            }
                            catch (NotCustomisableException e)
                            {
                                t.Warning(e.Message);
                            }
                        }

                        if (formXml != null)
                        {
                            FormHelper.UpdateFormXml(client, entity.SchemaName, formXml, "Information");   
                        }
                    }

                    if (entity.EntityPermissions == null)
                    {
                        t.Debug($"Entity {entity.DisplayName} has no security permissions to process");
                    }
                    else
                    {
                        // TODO - extract this out into a new method (and refactor the CreateSecurityRoles method too)
                        // TODO - only does Create at the moment... in refactored method, support all permissions
                        foreach (var entityPermission in entity.EntityPermissions)
                        {
                            var role = GetExistingSecurityRoleQuery(entityPermission.Name,
                                XrmClient.GetRootBusinessUnit(client)).RetrieveSingleRecord(client);
                            
                            var retrievePrivilegesByName = new QueryExpression("privilege")
                            {
                                ColumnSet = new ColumnSet(true),
                                Criteria = new FilterExpression
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression("name", ConditionOperator.Equal, $"prvCreate{entity.SchemaName}")
                                    }
                                }
                            };
                            var privilegeId = retrievePrivilegesByName.RetrieveSingleRecord(client);
                            if (privilegeId == null)
                            {
                                t.Warning($"Privilege with name prvCreate{entity.SchemaName} cannot be found. " +
                                          $"prvCreate{entity.SchemaName} for role {entityPermission.Name} has been skipped");
                                continue;
                            }

                            var addedPrivilege = new AddPrivilegesRoleRequest()
                            {
                                RoleId = role.Id,
                                Privileges = new []
                                {
                                    new RolePrivilege()
                                    {
                                        PrivilegeId = privilegeId.Id,
                                        Depth = PrivilegeDepth.Global
                                    }
                                }
                            };
                            client.Execute(addedPrivilege);
                            t.Debug($"{entityPermission.Name} role updated with permissions for entity {entity.DisplayName}");
                        }
                    }

                    t.Info($"Entity {entity.DisplayName} successfully processed");
                }
            }
        }

        /// <summary>
        /// Create or update model driven apps and related sitemaps
        /// </summary>
        /// <param name="manifest"></param>
        /// <param name="client"></param>
        /// <param name="t"></param>
        /// <param name="publisherPrefix"></param>
        public void CreateModelDrivenApps(ConfigurationManifest manifest, IOrganizationService client, TracingHelper t, string publisherPrefix)
        {

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

        private QueryBase GetExistingSecurityRoleQuery(string roleName, EntityReference businessUnit)
        {
            return new QueryExpression()
            {
                EntityName = Role.EntityLogicalName,
                ColumnSet = new ColumnSet(Role.Fields.Name),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(Role.Fields.Name, ConditionOperator.Equal, roleName),
                        new ConditionExpression(Role.Fields.BusinessUnitId, ConditionOperator.Equal, businessUnit.Id)
                    }
                }
            };
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
