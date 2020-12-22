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

        [XmlArrayItem("Step")]
        public CdsPluginStep[] Steps { get; set; }

        public EntityReference Register(IOrganizationService client, EntityReference parentAssembly)
        {
            if (parentAssembly.LogicalName != PluginAssembly.EntityLogicalName)
                throw new Exception($"Entity Reference '{nameof(parentAssembly)}' must be of type '{PluginAssembly.EntityLogicalName}'");

            var pluginType = new PluginType()
            {
                PluginAssemblyId = parentAssembly,
                TypeName = this.Name,
                FriendlyName = this.FriendlyName,
                Name = this.Name,
                Description = this.Description
            };

            var existingPluginQuery = this.GetExistingQuery(parentAssembly.Id);

            return pluginType.CreateOrUpdate(client, existingPluginQuery);

        }

        public void Unregister()
        {
            throw new NotImplementedException();
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
