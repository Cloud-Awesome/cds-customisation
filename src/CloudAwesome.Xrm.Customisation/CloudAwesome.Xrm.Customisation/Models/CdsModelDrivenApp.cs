using System.Xml.Serialization;

namespace CloudAwesome.Xrm.Customisation.Models
{
    public class CdsModelDrivenApp
    {
        public string Name { get; set; }

        public string UniqueName { get; set; }

        public string Description { get; set; }

        public CdsSiteMap SiteMap { get; set; }
    }
}
