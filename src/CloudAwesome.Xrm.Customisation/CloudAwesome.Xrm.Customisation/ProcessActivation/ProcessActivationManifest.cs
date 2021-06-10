using System.Xml.Serialization;
using CloudAwesome.Xrm.Core.Models;
using CloudAwesome.Xrm.Customisation.Models;

namespace CloudAwesome.Xrm.Customisation.ProcessActivation
{
    public enum ProcessActivationStatus { Enabled = 1, Disabled = 2 }

    public class ProcessActivationManifest: ICustomisationManifest
    {
        public ProcessActivationStatus Status { get; set; }
        public CdsConnection CdsConnection { get; set; }
        public LoggingConfiguration LoggingConfiguration { get; set; }

        [XmlArrayItem("PluginAssembly")]
        public CdsPluginAssembly[] PluginAssemblies { get; set; }

        [XmlArrayItem("Solution")]
        public CdsSolution[] Solutions { get; set; }

        [XmlArrayItem("Entity")]
        public CdsEntity[] Entities { get; set; }

        [XmlArrayItem("Workflow")]
        public string[] Workflows { get; set; }

        [XmlArrayItem("ModernFlow")]
        public string[] ModernFlows { get; set; }

        [XmlArrayItem("CaseCreationRule")]
        public string[] RecordCreationRules { get; set; }

    }
}
