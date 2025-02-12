using CloudAwesome.Xrm.Customisation.Models;
using CommandLine;

namespace CloudAwesome.Xrm.Customisation.Cli.CommandLineVerbs
{
    public abstract class BaseCliVerb
    {
        /// <summary>
        /// Allows user to enter connection details through commandline instead of being included in manifest
        /// </summary>
        internal readonly CdsConnection CdsConnectionDetails = new CdsConnection();

        internal bool OverrideManifestConnectionDetails = false;

        public abstract void Run(string manifestPath, CdsConnection cdsConnection = null);
        
        /// <summary>
        /// Commandline param: File path to the manifest
        /// </summary>
        [Option('m', "manifest", Required = true,
            HelpText = "File path to the XML manifest for the selected action")]
        public string Manifest { get; set; }

        [Option("connection-type", 
            HelpText = "Required if you want to override the CdsConnection details in manifest")]
        public CdsConnectionType ConnectionType
        {
            set
            {
                CdsConnectionDetails.ConnectionType = value;
                OverrideManifestConnectionDetails = true;
            }
        }

        [Option("url")]
        public string Url
        {
            set => CdsConnectionDetails.CdsUrl = value;
        }
        
        [Option("app-id")]
        public string AppId
        {
            set => CdsConnectionDetails.CdsAppId = value;
        }
        
        [Option("app-secret")]
        public string AppSecret
        {
            set => CdsConnectionDetails.CdsAppSecret = value;
        }
        
        [Option("user-name")]
        public string UserName
        {
            set => CdsConnectionDetails.CdsUserName = value;
        }
        
        [Option("password")]
        public string UserPassword
        {
            set => CdsConnectionDetails.CdsUserName = value;
        }
        
        [Option("connection-string")]
        public string ConnectionString
        {
            set => CdsConnectionDetails.CdsConnectionString = value;
        }

        [Option("filepath")]
        public string FilePath { get; set; }
    }
}