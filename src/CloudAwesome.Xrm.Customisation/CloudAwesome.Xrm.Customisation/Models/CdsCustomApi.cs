using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using CloudAwesome.Xrm.Customisation.DataverseExtensions;
using CloudAwesome.Xrm.Customisation.EarlyBoundModels;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Customisation.Models
{
    public enum ApiBindingType { Global, Entity, EntityCollection }

    public enum CustomProcessingStepType { None, AsyncOnly, SyncAndAsync }

    public class CdsCustomApi
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("friendlyName")]
        public string FriendlyName { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("executePrivilegeName")]
        public string ExecutePrivilegeName { get; set; }

        [JsonPropertyName("isFunction")]
        public bool? IsFunction { get; set; }

        [JsonPropertyName("isPrivate")]
        public bool? IsPrivate { get; set; }
        
        public EntityReference ParentPlugin { get; set; }

        [JsonPropertyName("allowedCustomProcessingStepName")]
        public CustomAPI_AllowedCustomProcessingStepType AllowedCustomProcessingStepType { get; set; }

        [JsonPropertyName("bindingType")]
        public CustomAPI_BindingType BindingType { get; set; }

        [JsonPropertyName("requestParameters")]
        [XmlArrayItem("RequestParameter")]
        public CdsRequestParameter[] RequestParameters { get; set; }

        [JsonPropertyName("responseProperties")]
        [XmlArrayItem("ResponseProperty")]
        public CdsResponseProperty[] ResponseProperties { get; set; }

        public EntityReference Register(IOrganizationService client, EntityReference parentPlugin)
        {
            var apiEntity = new CustomAPI()
            {
                Name = this.Name,
                UniqueName = this.Name,
                DisplayName = this.FriendlyName,
                Description = this.Description,
                PluginTypeId = parentPlugin,
                IsFunction = this.IsFunction,
                IsPrivate = this.IsPrivate,
                BindingType = this.BindingType,
                AllowedCustomProcessingStepType = this.AllowedCustomProcessingStepType,
                ExecutePrivilegeName = this.ExecutePrivilegeName
            };

            var existingApiQuery = this.GetExistingQuery(parentPlugin.Id);
            return apiEntity.CreateOrUpdate(client, existingApiQuery);
        }

        public bool Unregister(IOrganizationService client, EntityReference parentPlugin)
        {
            if (parentPlugin.LogicalName != PluginAssembly.EntityLogicalName)
                throw new ArgumentException($"Entity Reference '{nameof(parentPlugin)}' must be of type '{PluginType.EntityLogicalName}'");

            this.ParentPlugin = parentPlugin;
            return this.Unregister(client);
        }

        public bool Unregister(IOrganizationService client)
        {
            throw new NotImplementedException();
        }

        public QueryBase GetExistingQuery(Guid parentPluginId)
        {
            return new QueryExpression()
            {
                EntityName = CustomAPI.EntityLogicalName,
                ColumnSet = new ColumnSet(CustomAPI.PrimaryIdAttribute,
                    CustomAPI.PrimaryNameAttribute),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(CustomAPI.PrimaryNameAttribute,
                            ConditionOperator.Equal, this.Name),
                        new ConditionExpression(CustomAPI.Fields.PluginTypeId,
                            ConditionOperator.Equal, parentPluginId)
                    }
                }
            };
        }
    }
}
