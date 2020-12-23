using System.Xml.Serialization;

namespace CloudAwesome.Xrm.Customisation.Models
{
    public class CdsSiteMap
    {
        [XmlArrayItem("Area")]
        public CdsSiteMapArea[] Areas { get; set; }
    }
}
