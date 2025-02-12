using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using CloudAwesome.Xrm.Customisation.DataverseExtensions;
using CloudAwesome.Xrm.Customisation.EarlyBoundModels;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;

namespace CloudAwesome.Xrm.Customisation.Models
{
    [JsonObject]
    public class CdsPlugin
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("friendlyName")]
        public string FriendlyName { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        public EntityReference ParentAssembly { get; set; }

        [JsonPropertyName("steps")]
        [XmlArrayItem("Step")]
        public CdsPluginStep[] Steps { get; set; }

        [JsonPropertyName("customApis")]
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
