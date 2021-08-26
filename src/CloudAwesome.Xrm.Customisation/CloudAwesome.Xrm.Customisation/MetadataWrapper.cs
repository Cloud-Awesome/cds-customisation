using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace CloudAwesome.Xrm.Customisation
{
    public static class MetadataWrapper
    {
        public static EntityMetadata[] GetAllEntityMetadata(IOrganizationService client,
            EntityFilters filters = (EntityFilters.Entity 
                                     | EntityFilters.Attributes 
                                     | EntityFilters.Relationships
                                     | EntityFilters.Privileges))
        {
            var allEntitiesRequest = new RetrieveAllEntitiesRequest
            {
                EntityFilters = filters
            };
            var allEntitiesResponse = (RetrieveAllEntitiesResponse)client.Execute(allEntitiesRequest);
            
            return allEntitiesResponse.EntityMetadata;
        }
        
    }
}