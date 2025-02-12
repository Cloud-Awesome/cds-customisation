using System.Text.Json.Serialization;
using System.Xml.Serialization;
using CloudAwesome.Xrm.Customisation.Models;
using Newtonsoft.Json;

namespace CloudAwesome.Xrm.Customisation.PluginRegistration
{
    [JsonObject]
    public class PluginManifest: ICustomisationManifest
    {
        [JsonPropertyName("$schema")]
        public string JsonSchema { get; set; }
        
        [JsonPropertyName("solutionName")]
        [JsonInclude]
        public string SolutionName { get; set; }
        
        [JsonPropertyName("clobber")]
        [JsonInclude]
        public bool Clobber { get; set; }
        
        [JsonPropertyName("updateAssemblyOnly")]
        public bool UpdateAssemblyOnly { get; set; }

        [JsonPropertyName("logging")]
        public LoggingConfiguration LoggingConfiguration { get; set; }

        [JsonPropertyName("cdsConnection")]
        public CdsConnection CdsConnection { get; set; }

        [JsonPropertyName("assemblies")]
        [XmlArrayItem("PluginAssembly")]
        public CdsPluginAssembly[] PluginAssemblies { get; set; }

        [JsonPropertyName("serviceEndpoints")]
        [XmlArrayItem("ServiceEndpoint")]
        public CdsServiceEndpoint[] ServiceEndpoints { get; set; }

        [JsonPropertyName("webhooks")]
        [XmlArrayItem("Webhook")]
        public CdsWebhook[] Webhooks { get; set; }
    }
}
