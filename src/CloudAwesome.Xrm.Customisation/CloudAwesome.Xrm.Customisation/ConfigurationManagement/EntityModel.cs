using System.Xml;
using CloudAwesome.Xrm.Customisation.DataverseExtensions;
using CloudAwesome.Xrm.Customisation.Exceptions;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation.ConfigurationManagement
{
    public static class EntityModel
    {
        /// <summary>
        /// Create entities and attributes from a ConfigurationManifest
        /// </summary>
        /// <param name="manifest"></param>
        /// <param name="client"></param>
        /// <param name="t"></param>
        /// <param name="publisherPrefix"></param>
        public static void Generate(ConfigurationManifest manifest, IOrganizationService client, TracingHelper t, string publisherPrefix)
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

                                if (attribute.AddToForm)
                                {
                                    FormHelper.AddAttributeToForm(attributeMetaData, ref formXml);
                                    t.Debug($"Attribute {attribute.DisplayName} added to form");
                                }

                                // TODO - add to views
                                if (attribute.AddToViewOrder.HasValue)
                                {
                                    attribute.AddToSystemViews();
                                    t.Debug($"Attribute {attribute.DisplayName} added to views");
                                }
                                
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

                        var rootBusinessUnit = XrmClient.GetRootBusinessUnit(client);
                        
                        foreach (var entityPermission in entity.EntityPermissions)
                        {
                            SecurityRoles.UpdateRoleEntityPermission(client, entity.SchemaName, entityPermission.RoleName,
                                "Create", entityPermission.Create, rootBusinessUnit, t);
                            SecurityRoles.UpdateRoleEntityPermission(client, entity.SchemaName, entityPermission.RoleName,
                                "Read", entityPermission.Read, rootBusinessUnit, t);
                            SecurityRoles.UpdateRoleEntityPermission(client, entity.SchemaName, entityPermission.RoleName,
                                "Write", entityPermission.Write, rootBusinessUnit, t);
                            SecurityRoles.UpdateRoleEntityPermission(client, entity.SchemaName, entityPermission.RoleName,
                                "Delete", entityPermission.Delete, rootBusinessUnit, t);
                            SecurityRoles.UpdateRoleEntityPermission(client, entity.SchemaName, entityPermission.RoleName,
                                "Append", entityPermission.Append, rootBusinessUnit, t);
                            SecurityRoles.UpdateRoleEntityPermission(client, entity.SchemaName, entityPermission.RoleName,
                                "AppendTo", entityPermission.AppendTo, rootBusinessUnit, t);
                            SecurityRoles.UpdateRoleEntityPermission(client, entity.SchemaName, entityPermission.RoleName,
                                "Share", entityPermission.Share, rootBusinessUnit, t);
                        }
                    }

                    t.Info($"Entity {entity.DisplayName} successfully processed");
                }
            }
        }
    }
}