using System;
using System.Xml.Serialization;
using CloudAwesome.Xrm.Core;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Customisation.Models
{
    public enum ApiBindingType { Global, Entity, EntityCollection }

    public enum CustomProcessingStepType { None, AsyncOnly, SyncAndAsync }

    public class CdsCustomApi
    {
        public string Name { get; set; }

        public string FriendlyName { get; set; }

        public string Description { get; set; }

        public string ExecutePrivilegeName { get; set; }

        public bool? IsFunction { get; set; }

        public bool? IsPrivate { get; set; }

        public CustomAPI_AllowedCustomProcessingStepType AllowedCustomProcessingStepType { get; set; }

        public CustomAPI_BindingType BindingType { get; set; }

        [XmlArrayItem("RequestParameter")]
        public CdsRequestParameter[] RequestParameters { get; set; }

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
