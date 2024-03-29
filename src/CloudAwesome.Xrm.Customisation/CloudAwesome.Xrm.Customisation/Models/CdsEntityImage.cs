﻿using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Customisation.EarlyBoundModels;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;

namespace CloudAwesome.Xrm.Customisation.Models
{
    public enum EntityImageType { PreImage, PostImage }

    [JsonObject]
    public class CdsEntityImage
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("postImage")]
        public bool PostImage { get; set; }

        [JsonPropertyName("preImage")]
        public bool PreImage { get; set; }
        
        public EntityImageType Type { get; set; }

        [JsonPropertyName("attributes")]
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
