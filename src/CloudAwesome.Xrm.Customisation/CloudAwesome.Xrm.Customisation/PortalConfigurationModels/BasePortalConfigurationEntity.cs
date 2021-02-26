using System;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation.PortalConfigurationModels
{
    public abstract class BasePortalConfigurationEntity
    {
        public string EntityName = string.Empty;
        
        public EntityReference CreateOrUpdate()
        {
            throw new NotImplementedException();
        }
    }
}