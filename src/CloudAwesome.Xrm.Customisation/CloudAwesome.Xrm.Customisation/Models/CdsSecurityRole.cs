using System.Xml.Serialization;

namespace CloudAwesome.Xrm.Customisation.Models
{
    public class CdsSecurityRole
    {
        public string Name { get; set; }

        [XmlArrayItem("Privilege")]
        public string[] Privileges { get; set; }
    }
}
