using System.Xml.Serialization;

namespace CloudAwesome.Xrm.Customisation.Models
{
    public class CdsSiteMapGroup
    {
        public string Name { get; set; }

        [XmlArrayItem("SubArea")]
        public CdsSiteMapSubArea[] SubAreas { get; set; }
    }
}
