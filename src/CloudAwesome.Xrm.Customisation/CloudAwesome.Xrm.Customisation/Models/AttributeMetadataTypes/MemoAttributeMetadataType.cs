﻿using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace CloudAwesome.Xrm.Customisation.Models.AttributeMetadataTypes
{
    public class MemoAttributeMetadataType: IAttributeMetadataType
    {
        private readonly CdsAttribute _attribute;
        private readonly string _publisherPrefix;
        private readonly AttributeMetadata _existingMetadata;

        public MemoAttributeMetadataType(CdsAttribute attribute, string publisherPrefix, AttributeMetadata existingMetadata = null)
        {
            _attribute = attribute;
            _publisherPrefix = publisherPrefix;
            _existingMetadata = existingMetadata;
        }

        public string Name => nameof(MemoAttributeMetadataType);
        
        public AttributeMetadata AttributeMetadata
        {
            get
            {
                MemoAttributeMetadata attributeMetadata;
                if (_existingMetadata == null)
                {
                    // Create
                    attributeMetadata = new MemoAttributeMetadata()
                    {
                        LogicalName = _attribute.SchemaName,
                        SchemaName = _attribute.SchemaName,
                        DisplayName = _attribute.DisplayName.CreateLabelFromString(),
                        Description = _attribute.Description.CreateLabelFromString(),
                        RequiredLevel = new AttributeRequiredLevelManagedProperty(_attribute.RequiredLevel),
                        IsAuditEnabled = new BooleanManagedProperty(_attribute.IsAuditEnabled),
                        MaxLength = _attribute.MaxLength,
                        Format = StringFormat.TextArea
                    };
                }
                else
                {
                    attributeMetadata = (MemoAttributeMetadata) _existingMetadata;
                    attributeMetadata.DisplayName = _attribute.DisplayName.CreateLabelFromString();
                    attributeMetadata.Description = string.IsNullOrEmpty(_attribute.Description)
                        ? attributeMetadata.Description
                        : _attribute.Description.CreateLabelFromString();
                    attributeMetadata.RequiredLevel = new AttributeRequiredLevelManagedProperty(_attribute.RequiredLevel);
                    attributeMetadata.IsAuditEnabled = new BooleanManagedProperty(_attribute.IsAuditEnabled);
                    attributeMetadata.MaxLength = _attribute.MaxLength ?? attributeMetadata.MaxLength;
                    attributeMetadata.Format = StringFormat.TextArea;
                }
                
                return attributeMetadata;
            }
        }
    }
}
