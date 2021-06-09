using System;
using System.Xml.Serialization;
using CloudAwesome.Xrm.Core;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Customisation.Models
{
    public class CdsPlugin
    {
        public string Name { get; set; }

        public string FriendlyName { get; set; }

        public string Description { get; set; }

        public EntityReference ParentAssembly { get; set; }

        [XmlArrayItem("Step")]
        public CdsPluginStep[] Steps { get; set; }

        [XmlArrayItem("CustomApi")]
        public CdsCustomApi[] CustomApis { get; set; }

        public EntityReference Register(IOrganizationService client, EntityReference parentAssembly)
        {
            if (parentAssembly.LogicalName != PluginAssembly.EntityLogicalName)
                throw new ArgumentException($"Entity Reference '{nameof(parentAssembly)}' must be of type '{PluginAssembly.EntityLogicalName}'");

            this.ParentAssembly = parentAssembly;
            return this.Register(client);
        }

        public EntityReference Register(IOrganizationService client)
        {
            var pluginType = new PluginType()
            {
                PluginAssemblyId = this.ParentAssembly,
                TypeName = this.Name,
                FriendlyName = this.FriendlyName,
                Name = this.Name,
                Description = this.Description
            };

            var existingPluginQuery = this.GetExistingQuery(this.ParentAssembly.Id);
            return pluginType.CreateOrUpdate(client, existingPluginQuery);
        }

        public void Unregister(IOrganizationService client, EntityReference parentAssembly)
        {
            if (parentAssembly.LogicalName != PluginAssembly.EntityLogicalName)
                throw new ArgumentException($"Entity Reference '{nameof(parentAssembly)}' must be of type '{PluginAssembly.EntityLogicalName}'");

            this.ParentAssembly = parentAssembly;
            this.Unregister(client);
        }

        public void Unregister(IOrganizationService client)
        {
            throw new NotImplementedException("Issue #37");
        }

        public QueryBase GetExistingQuery(Guid parentAssemblyId)
        {
            return new QueryExpression()
            {
                EntityName = PluginType.EntityLogicalName,
                ColumnSet = new ColumnSet(PluginType.PrimaryIdAttribute, 
                    PluginType.PrimaryNameAttribute),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(PluginType.PrimaryNameAttribute, 
                            ConditionOperator.Equal, this.Name),
                        new ConditionExpression(PluginType.Fields.PluginAssemblyId, 
                            ConditionOperator.Equal, parentAssemblyId)
                    }
                }
            };
        }
    }
}
