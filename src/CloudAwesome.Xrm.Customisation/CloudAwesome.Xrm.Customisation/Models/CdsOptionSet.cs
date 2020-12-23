using System.Xml.Serialization;

namespace CloudAwesome.Xrm.Customisation.Models
{
    public class CdsOptionSet
    {
        public string DisplayName { get; set; }
        public string SchemaName { get; set; }

        [XmlArrayItem("Item")]
        public string[] Items { get; set; }
    }
}
