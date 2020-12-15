using System.Xml.Serialization;

namespace CloudAwesome.Xrm.Customisation.Models
{
    public enum EntityImageType { PreImage, PostImage }

    public class CdsEntityImage
    {
        public string Name { get; set; }

        public EntityImageType Type { get; set; }

        [XmlArrayItem("Attribute")]
        public string[] Attributes { get; set; }
    }
}
