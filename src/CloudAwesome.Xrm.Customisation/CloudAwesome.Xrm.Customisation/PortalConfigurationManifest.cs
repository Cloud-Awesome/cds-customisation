using System.Collections.Generic;
using CloudAwesome.Xrm.Core.Models;
using CloudAwesome.Xrm.Customisation.PortalConfigurationModels;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation
{
    public class PortalConfigurationManifest
    {
        public CdsConnection CdsConnection { get; set; }
        public LoggingConfiguration LoggingConfiguration { get; set; }
        
        public string Website { get; set; }
        
        public PortalWebPage[] WebPages { get; set; }
        
        public PortalPageTemplate[] PageTemplates { get; set; }
        
        public PortalEntityForm[] EntityForms { get; set; }
        
        public PortalEntityList[] EntityLists { get; set; }
        
        public PortalContentSnippet[] ContentSnippets { get; set; }
        
        public PortalSiteSetting[] SiteSettings { get; set; }
        
        public PortalWebLinkSet[] WebLinkSets { get; set; }
        
        public PortalWebTemplate[] WebTemplates { get; set; }
        
        public PortalWebFile[] WebFiles { get; set; }
        
        public PortalEntityPermission[] EntityPermissions { get; set; }

        public PortalWebRole[] WebRoles { get; set; }

    }
}