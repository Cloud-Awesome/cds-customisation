using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation.PortalConfigurationModels
{
    public interface IPortalConfigurationEntity
    {
        EntityReference CreateOrUpdate(IOrganizationService client);

    }
}