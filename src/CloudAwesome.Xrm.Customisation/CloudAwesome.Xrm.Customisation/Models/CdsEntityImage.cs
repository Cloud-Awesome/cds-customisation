using System;
using System.Xml.Serialization;
using CloudAwesome.Xrm.Core;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Customisation.Models
{
    public enum EntityImageType { PreImage, PostImage }

    public class CdsEntityImage
    {
        public string Name { get; set; }

        public EntityImageType Type { get; set; }

        [XmlArrayItem("Attribute")]
        public string[] Attributes { get; set; }

        public EntityReference Register(IOrganizationService client, EntityReference parentStep)
        {
            var stepImage = new SdkMessageProcessingStepImage()
            {
                Name = this.Name,
                EntityAlias = this.Name,
                Attributes1 = string.Join(",", this.Attributes),
                ImageType = SdkMessageProcessingStepImage_ImageType.PreImage,
                MessagePropertyName = "Target",
                SdkMessageProcessingStepId = parentStep
            };

            var existingImageQuery = this.GetExistingQuery(parentStep);

            return stepImage.CreateOrUpdate(client, existingImageQuery);
        }

        public void Unregister()
        {
            throw new NotImplementedException();
        }

        public QueryBase GetExistingQuery(EntityReference parentStep)
        {
            return new QueryExpression(SdkMessageProcessingStepImage.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(SdkMessageProcessingStepImage.PrimaryIdAttribute,
                    SdkMessageProcessingStep.PrimaryNameAttribute),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(SdkMessageProcessingStepImage.PrimaryNameAttribute,
                            ConditionOperator.Equal, this.Name),
                        new ConditionExpression(SdkMessageProcessingStep.PrimaryIdAttribute,
                            ConditionOperator.Equal, parentStep.Id)
                    }
                }
            };
        }
    }
}
