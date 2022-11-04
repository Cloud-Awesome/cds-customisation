using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Customisation.EarlyBoundModels;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Customisation.ConfigurationManagement
{
    public static class SecurityRoles
    {
        /// <summary>
        /// Create or update security roles
        /// </summary>
        /// <param name="manifest"></param>
        /// <param name="client"></param>
        /// <param name="t"></param>
        /// <param name="publisherPrefix"></param>
        public static void Generate(ConfigurationManifest manifest, IOrganizationService client, TracingHelper t, string publisherPrefix)
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
        
        public static void UpdateRoleEntityPermission(IOrganizationService client, string entitySchemaName,
            string securityRoleName, string privilege, PrivilegeDepth? privilegeDepth, EntityReference rootBusinessUnit,
            TracingHelper t)
        {
            if (privilegeDepth == null) return;

            var entityPrivilegeName = $"prv{privilege}{entitySchemaName}";
            var role = GetExistingSecurityRoleQuery(securityRoleName, rootBusinessUnit)
                .RetrieveSingleRecord(client);

            var retrievePrivilegesByName = new QueryExpression("privilege")
            {
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("name",
                            ConditionOperator.Equal, $"{entityPrivilegeName}")
                    }
                }
            };
            
            var privilegeId = retrievePrivilegesByName.RetrieveSingleRecord(client);
            if (privilegeId == null)
            {
                t.Warning($"Privilege with name {entityPrivilegeName} cannot be found. " +
                          $"{entityPrivilegeName} for role {securityRoleName} has been skipped");
                return;
            }

            var addedPrivilege = new AddPrivilegesRoleRequest()
            {
                RoleId = role.Id,
                Privileges = new[]
                {
                    new RolePrivilege()
                    {
                        PrivilegeId = privilegeId.Id,
                        Depth = privilegeDepth.Value
                    }
                }
            };
            client.Execute(addedPrivilege);
            t.Debug($"{securityRoleName} role updated with permissions for entity {entityPrivilegeName}: {privilegeDepth.Value}");
        }
        
        private static QueryBase GetExistingSecurityRoleQuery(string roleName, EntityReference businessUnit)
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

    }
}