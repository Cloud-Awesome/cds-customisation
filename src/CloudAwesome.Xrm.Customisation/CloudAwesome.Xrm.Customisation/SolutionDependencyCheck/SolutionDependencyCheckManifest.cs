using System.Text.Json.Serialization;
using CloudAwesome.Xrm.Customisation.Models;
using Newtonsoft.Json;

namespace CloudAwesome.Xrm.Customisation.SolutionDependencyCheck
{
    [JsonObject]
    public class SolutionDependencyCheckManifest: ICustomisationManifest
    {
        [JsonPropertyName("$schema")]
        public string JsonSchema { get; set; }
        
        [JsonPropertyName("solutionFilepath")]
        public string SolutionFilepath { get; set; }
        
        [JsonPropertyName("logging")]
        public LoggingConfiguration LoggingConfiguration { get; set; }

        [JsonPropertyName("cdsConnection")]
        public CdsConnection CdsConnection { get; set; }
        
        [JsonPropertyName("solutionFilePath")]
        public string SolutionPath { get; set; }
    }
}
