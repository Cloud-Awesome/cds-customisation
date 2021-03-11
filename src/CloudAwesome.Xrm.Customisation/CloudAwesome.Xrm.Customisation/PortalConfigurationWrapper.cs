using System.Collections.Generic;
using System.Linq;
using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Customisation.Exceptions;
using CloudAwesome.Xrm.Customisation.PortalConfigurationModels;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation
{
    public class PortalConfigurationWrapper
    {
        public void GenerateCustomisations(PortalConfigurationManifest manifest, 
            IOrganizationService client, TracingHelper t)
        {
            if (string.IsNullOrEmpty(manifest.Website))
            {
                throw new InvalidManifestException("Website is not specified in the manifest");
            }

            var entitiesToProcess = new List<IPortalConfigurationEntity[]>()
            {
                manifest.EntityForms, manifest.EntityLists,
                manifest.EntityPermissions, manifest.PageTemplates,
                manifest.SiteSettings, manifest.WebFiles, 
                manifest.WebPages, manifest.WebRoles, manifest.WebTemplates,
                manifest.WebLinkSets
            };

            foreach (var portalConfigurationEntity 
                in entitiesToProcess.SelectMany(test => test))
            {
                portalConfigurationEntity.CreateOrUpdate(client);
            }
            
        }
    }
}