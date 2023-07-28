using System;
using System.Text.Json.Serialization;
using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Customisation.EarlyBoundModels;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Customisation.Models
{
    public class CdsRequestParameter
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("friendlyName")]
        public string FriendlyName { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("logicalEntityName")]
        public string LogicalEntityName { get; set; }

        [JsonPropertyName("isOptional")]
        public bool? IsOptional { get; set; }
        
        [JsonPropertyName("type")]
        public CustomAPIFieldType Type { get; set; }

        public EntityReference Register(IOrganizationService client, EntityReference parentCustomApi)
        {
            var responseProperty = new CustomAPIRequestParameter()
            {
                Name = this.Name,
                UniqueName = this.Name,
                DisplayName = this.FriendlyName,
                Description = this.Description,
                Type = this.Type,
                LogicalEntityName = this.LogicalEntityName,
                IsOptional = this.IsOptional,
                CustomAPIId = parentCustomApi
            };

            var existingQuery = this.GetExistingQuery(parentCustomApi.Id);

            return responseProperty.CreateOrUpdate(client, existingQuery);

        }

        public QueryBase GetExistingQuery(Guid parentCustomApi)
        {
            return new QueryExpression()
            {
                EntityName = CustomAPIRequestParameter.EntityLogicalName,
                ColumnSet = new ColumnSet(CustomAPIRequestParameter.PrimaryIdAttribute,
                    CustomAPIRequestParameter.PrimaryNameAttribute),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(CustomAPIRequestParameter.PrimaryNameAttribute,
                            ConditionOperator.Equal, this.Name),
                        new ConditionExpression(CustomAPIRequestParameter.Fields.CustomAPIId,
                            ConditionOperator.Equal, parentCustomApi)
                    }
                }
            };
        }
    }
}