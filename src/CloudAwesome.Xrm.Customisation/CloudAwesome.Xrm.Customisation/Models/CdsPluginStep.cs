using System;
using System.Xml.Serialization;
using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Customisation.Exceptions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Customisation.Models
{
    public class CdsPluginStep
    {
        public string Name { get; set; }

        public string FriendlyName { get; set; }

        public string Description { get; set; }

        public SdkMessageProcessingStep_Stage Stage { get; set; }

        public SdkMessageProcessingStep_Mode ExecutionMode { get; set; }

        public string Message { get; set; }

        public string PrimaryEntity { get; set; }

        public int ExecutionOrder { get; set; }

        public bool AsyncAutoDelete { get; set; }

        public string UnsecureConfiguration { get; set; }

        public string SecureConfiguration { get; set; }

        [XmlArrayItem("Attribute")]
        public string[] FilteringAttributes { get; set; }

        [XmlArrayItem("EntityImage")]
        public CdsEntityImage[] EntityImages { get; set; }

        public EntityReference Register(IOrganizationService client, EntityReference parentPluginType,
            EntityReference sdkMessage, EntityReference sdkMessageFilter)
        {
            var step = new SdkMessageProcessingStep()
            {
                Name = this.Name,
                Configuration = string.IsNullOrEmpty(this.UnsecureConfiguration)
                    ? this.UnsecureConfiguration
                    : "",
                Mode = this.ExecutionMode,
                Rank = this.ExecutionOrder,
                Stage = this.Stage,
                SupportedDeployment = SdkMessageProcessingStep_SupportedDeployment.ServerOnly, // Only ServerOnly supported
                EventHandler = parentPluginType,
                SdkMessageId = sdkMessage,
                Description = this.Description,
                AsyncAutoDelete = this.AsyncAutoDelete,
                SdkMessageFilterId = sdkMessageFilter
            };

            if (this.FilteringAttributes != null && this.FilteringAttributes.Length > 0)
            {
                step.FilteringAttributes = string.Join(",", this.FilteringAttributes);
            }

            var existingStepQuery = this.GetExistingQuery(parentPluginType.Id, sdkMessage.Id, sdkMessageFilter.Id);
            return step.CreateOrUpdate(client, existingStepQuery);
        }

        public void Unregister(IOrganizationService client, EntityReference parentPluginType)
        {
            throw new NotImplementedException();
        }

        public void ToggleStatus(IOrganizationService client, bool activate)
        {
            var status = activate
                ? SdkMessageProcessingStep_StatusCode.Enabled
                : SdkMessageProcessingStep_StatusCode.Disabled;

            var queryStatus = activate
                ? SdkMessageProcessingStep_StatusCode.Disabled
                : SdkMessageProcessingStep_StatusCode.Enabled;

            var state = activate
                ? SdkMessageProcessingStepState.Enabled
                : SdkMessageProcessingStepState.Disabled;

            var existingStep = new QueryExpression()
            {
                EntityName = SdkMessageProcessingStep.EntityLogicalName,
                ColumnSet = new ColumnSet(SdkMessageProcessingStep.PrimaryNameAttribute, 
                    SdkMessageProcessingStep.Fields.StatusCode),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageFilter.Fields.Name, 
                            ConditionOperator.Equal, this.Name),
                        new ConditionExpression(SdkMessageProcessingStep.Fields.StatusCode,
                            ConditionOperator.Equal, (int)queryStatus)
                    }
                }
            }.RetrieveSingleRecord(client);

            if (existingStep == null)
            {
                throw new NoProcessToUpdateException("Process either doesn't exist or already in the required state");
            }

            var step = new SdkMessageProcessingStep()
            {
                Id = existingStep.Id,
                StatusCode = status,
                StateCode = state
            };
            step.Update(client);
        }


        public QueryBase GetExistingQuery(Guid parentPluginType, Guid sdkMessage, Guid sdkMessageFilter)
        {
            return new QueryExpression(SdkMessageProcessingStep.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(SdkMessageProcessingStep.PrimaryIdAttribute,
                    SdkMessageProcessingStep.PrimaryNameAttribute),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageProcessingStep.Fields.EventHandler, ConditionOperator.Equal, parentPluginType),
                        new ConditionExpression(SdkMessageProcessingStep.Fields.SdkMessageId, ConditionOperator.Equal, sdkMessage),
                        new ConditionExpression(SdkMessageProcessingStep.Fields.SdkMessageFilterId,
                            ConditionOperator.Equal, sdkMessageFilter),
                        new ConditionExpression(SdkMessageProcessingStep.Fields.Stage, ConditionOperator.Equal,
                            (int)this.Stage),
                    }
                }
            };
        }
    }
}
