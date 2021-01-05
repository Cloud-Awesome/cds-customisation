using System;
using System.Collections.Generic;
using CloudAwesome.Xrm.Customisation.Models;
using CloudAwesome.Xrm.Customisation.Models.AttributeMetadataTypes;
using Microsoft.Xrm.Sdk.Metadata;

namespace CloudAwesome.Xrm.Customisation
{
    public class AttributeMetadataContext
    {
        public readonly Dictionary<CdsAttributeDataType, IAttributeMetadataType> Strategies = new Dictionary<CdsAttributeDataType, IAttributeMetadataType>();

        public AttributeMetadataContext(CdsAttribute attribute, string publisherPrefix, AttributeMetadata existingMetadata = null)
        {
            Strategies.Add(CdsAttributeDataType.Boolean, new BooleanAttributeMetadataType(attribute, publisherPrefix, existingMetadata));
            Strategies.Add(CdsAttributeDataType.DateTime, new DateTimeAttributeMetadataType(attribute, publisherPrefix, existingMetadata));
            Strategies.Add(CdsAttributeDataType.Integer, new IntegerAttributeMetadataType(attribute, publisherPrefix, existingMetadata));
            Strategies.Add(CdsAttributeDataType.Lookup, new LookupAttributeMetadataType(attribute, publisherPrefix, existingMetadata));
            Strategies.Add(CdsAttributeDataType.Memo, new MemoAttributeMetadataType(attribute, publisherPrefix, existingMetadata));
            Strategies.Add(CdsAttributeDataType.Picklist, new PicklistAttributeMetadataType(attribute, publisherPrefix, existingMetadata));
            Strategies.Add(CdsAttributeDataType.String, new StringAttributeMetadataType(attribute, publisherPrefix, existingMetadata));
        }

        public IAttributeMetadataType GetAttributeMetadataType(CdsAttributeDataType dataType)
        {
            var strategy = Strategies[dataType];
            return strategy;
        }
    }
}
