using System.Xml.Serialization;

namespace CloudAwesome.Xrm.Customisation.Models
{
    public class CdsSiteMapArea
    {
        public string Name { get; set; }

        [XmlArrayItem("Group")]
        public CdsSiteMapGroup[] Groups { get; set; }
    }
}
