using System.Xml.Serialization;

namespace CloudAwesome.Xrm.Customisation.Models
{
    public class CdsPlugin
    {
        public string Name { get; set; }

        public string FriendlyName { get; set; }

        public string Description { get; set; }

        [XmlArrayItem("Step")]
        public CdsPluginStep[] Steps { get; set; }
    }
}
