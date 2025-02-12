using System;
using System.Text.Json.Serialization;
using CloudAwesome.Xrm.Customisation.DataverseExtensions;
using CloudAwesome.Xrm.Customisation.EarlyBoundModels;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Customisation.Models
{
    public class CdsResponseProperty
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("friendlyName")]
        public string FriendlyName { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("type")]
        public CustomAPIFieldType Type { get; set; }

        [JsonPropertyName("logicalEntityName")]
        public string LogicalEntityName { get; set; }

        public EntityReference Register(IOrganizationService client, EntityReference parentCustomApi)
        {
            var responseProperty = new CustomAPIResponseProperty()
            {
                Name = this.Name,
                UniqueName = this.Name,
                DisplayName = this.FriendlyName,
                Description = this.Description,
                Type = this.Type,
                LogicalEntityName = this.LogicalEntityName,
                CustomAPIId = parentCustomApi
            };

            var existingQuery = this.GetExistingQuery(parentCustomApi.Id);

            return responseProperty.CreateOrUpdate(client, existingQuery);
        }

        public QueryBase GetExistingQuery(Guid parentCustomApi)
        {
            return new QueryExpression()
            {
                EntityName = CustomAPIResponseProperty.EntityLogicalName,
                ColumnSet = new ColumnSet(CustomAPIResponseProperty.PrimaryIdAttribute,
                    CustomAPIResponseProperty.PrimaryNameAttribute),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(CustomAPIResponseProperty.PrimaryNameAttribute,
                            ConditionOperator.Equal, this.Name),
                        new ConditionExpression(CustomAPIResponseProperty.Fields.CustomAPIId,
                            ConditionOperator.Equal, parentCustomApi)
                    }
                }
            };
        }
    }
}