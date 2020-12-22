﻿using System;
using System.IO;
using System.Xml.Serialization;
using CloudAwesome.Xrm.Core;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Customisation.Models
{
    public class CdsPluginAssembly
    {
        public string Name { get; set; }

        public string FriendlyName { get; set; }

        public string Assembly { get; set; }

        public string SolutionName { get; set; }

        [XmlArrayItem("Plugin")]
        public CdsPlugin[] Plugins { get; set; }

        public EntityReference Register(IOrganizationService client, PluginAssemblyInfo pluginAssemblyInfo = null)
        {
            if (pluginAssemblyInfo == null)
            {
                pluginAssemblyInfo = new PluginAssemblyInfo(this.Assembly);
            }

            var assemblyEntity = new PluginAssembly()
            {
                Name = this.Name,
                Culture = pluginAssemblyInfo.Culture,
                Version = pluginAssemblyInfo.Version,
                PublicKeyToken = pluginAssemblyInfo.PublicKeyToken,
                SourceType = PluginAssembly_SourceType.Database,// Only database supported for now
                IsolationMode = PluginAssembly_IsolationMode.Sandbox, // Only Sandbox supported for now
                Content = Convert.ToBase64String(File.ReadAllBytes(this.Assembly))
            };

            var existingAssemblyQuery = this.GetExistingQuery(pluginAssemblyInfo.Version);

            return assemblyEntity.CreateOrUpdate(client, existingAssemblyQuery);
        }

        public bool Unregister(IOrganizationService client, PluginAssemblyInfo pluginAssemblyInfo = null)
        {
            if (pluginAssemblyInfo == null)
            {
                pluginAssemblyInfo = new PluginAssemblyInfo(this.Assembly);
            }

            var existingAssembly = this.GetExistingQuery(pluginAssemblyInfo.Version).RetrieveSingleRecord(client);
            if (existingAssembly == null) return false;

            // Currently hard-coded to a max of 50 records to delete
            var deletedChildPlugins = new QueryExpression()
            {
                EntityName = PluginType.EntityLogicalName,
                ColumnSet = new ColumnSet(PluginType.Fields.Name),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(PluginType.Fields.PluginAssemblyId,
                            ConditionOperator.Equal, existingAssembly.Id)
                    }
                }
            };
            var result = deletedChildPlugins.DeleteAllResults(client);

            if (!result) return false;

            existingAssembly.Delete(client);
            return true;

        }

        public QueryBase GetExistingQuery(string assemblyVersion)
        {
            return new QueryExpression()
            {
                EntityName = PluginAssembly.EntityLogicalName,
                ColumnSet = new ColumnSet(PluginAssembly.PrimaryIdAttribute, PluginAssembly.PrimaryNameAttribute),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(PluginAssembly.PrimaryNameAttribute, 
                            ConditionOperator.Equal, this.Name),
                        new ConditionExpression(PluginAssembly.Fields.Version, 
                            ConditionOperator.Equal, assemblyVersion)
                    }
                }
            };
        }
    }
}
