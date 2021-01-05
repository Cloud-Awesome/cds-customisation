using Microsoft.Xrm.Sdk.Metadata;

namespace CloudAwesome.Xrm.Customisation.Models
{
    public interface IAttributeMetadataType
    {
        string Name { get; }
        AttributeMetadata AttributeMetadata { get; }
    }
}