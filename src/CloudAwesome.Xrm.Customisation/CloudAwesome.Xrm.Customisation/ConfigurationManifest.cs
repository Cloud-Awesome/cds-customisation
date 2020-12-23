using System.Xml.Serialization;
using CloudAwesome.Xrm.Core.Models;
using CloudAwesome.Xrm.Customisation.Models;

namespace CloudAwesome.Xrm.Customisation
{
    public class ConfigurationManifest
    {
        public string SolutionName { get; set; }
        public bool Clobber { get; set; }

        public LoggingConfiguration LoggingConfiguration { get; set; }

        public CdsConnection CdsConnection { get; set; }

        [XmlArrayItem("Entity")]
        public CdsEntity[] Entities { get; set; }

        [XmlArrayItem("OptionSet")]
        public CdsOptionSet[] OptionSets { get; set; }

        [XmlArrayItem("SecurityRole")]
        public CdsSecurityRole[] SecurityRoles { get; set; }

        [XmlArrayItem("ModelDrivenApp")]
        public CdsModelDrivenApp[] ModelDrivenApps { get; set; }

    }
}
