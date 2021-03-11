using System.Xml.Serialization;
using CloudAwesome.Xrm.Core.Models;
using CloudAwesome.Xrm.Customisation.PortalConfigurationModels;

namespace CloudAwesome.Xrm.Customisation
{
    public class PortalConfigurationManifest: ICustomisationManifest
    {
        public CdsConnection CdsConnection { get; set; }
        public LoggingConfiguration LoggingConfiguration { get; set; }
        
        public string Website { get; set; }
        
        [XmlArrayItem("WebPage")]
        public PortalWebPage[] WebPages { get; set; }
        
        [XmlArrayItem("PageTemplate")]
        public PortalPageTemplate[] PageTemplates { get; set; }
        
        [XmlArrayItem("EntityForm")]
        public PortalEntityForm[] EntityForms { get; set; }
        
        [XmlArrayItem("EntityList")]
        public PortalEntityList[] EntityLists { get; set; }
        
        [XmlArrayItem("ContentSnippet")]
        public PortalContentSnippet[] ContentSnippets { get; set; }
        
        [XmlArrayItem("SiteSetting")]
        public PortalSiteSetting[] SiteSettings { get; set; }
        
        [XmlArrayItem("WebLinkSet")]
        public PortalWebLinkSet[] WebLinkSets { get; set; }
        
        [XmlArrayItem("WebTemplate")]
        public PortalWebTemplate[] WebTemplates { get; set; }
        
        [XmlArrayItem("Webfile")]
        public PortalWebFile[] WebFiles { get; set; }
        
        [XmlArrayItem("EntityPermission")]
        public PortalEntityPermission[] EntityPermissions { get; set; }

        [XmlArrayItem("WebRole")]
        public PortalWebRole[] WebRoles { get; set; }

    }
}