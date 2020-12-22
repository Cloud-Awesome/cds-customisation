using System;
using System.Xml.Serialization;
using CloudAwesome.Xrm.Core;
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
            var sdkStep = new SdkMessageProcessingStep()
            {
                Name = this.Name,
                Configuration = this.UnsecureConfiguration,
                Mode = this.ExecutionMode,
                Rank = this.ExecutionOrder,
                Stage = this.Stage,
                SupportedDeployment = SdkMessageProcessingStep_SupportedDeployment.ServerOnly, // Only ServerOnly supported
                EventHandler = parentPluginType,
                SdkMessageId = sdkMessage,
                Description = this.Description,
                AsyncAutoDelete = this.AsyncAutoDelete,
                SdkMessageFilterId = sdkMessageFilter
                // TODO loop through attributes to create a single string? #3
                //FilteringAttributes = step.FilteringAttributes.
            };

            var existingStepQuery = this.GetExistingQuery(parentPluginType.Id, sdkMessage.Id);

            return sdkStep.CreateOrUpdate(client, existingStepQuery);
        }

        public void Unregister(IOrganizationService client, EntityReference parentPluginType)
        {
            throw new NotImplementedException();
        }

        public QueryBase GetExistingQuery(Guid parentPluginType, Guid sdkMessage)
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
                        new ConditionExpression(SdkMessageProcessingStep.Fields.Stage, ConditionOperator.Equal,
                            (int)SdkMessageProcessingStep_Stage.Postoperation),
                    }
                }
            };
        }
    }
}
