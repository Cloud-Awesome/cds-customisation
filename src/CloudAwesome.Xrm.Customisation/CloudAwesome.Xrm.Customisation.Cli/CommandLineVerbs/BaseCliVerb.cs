using System;
using CloudAwesome.Xrm.Core.Models;

namespace CloudAwesome.Xrm.Customisation.Cli
{
    using CommandLine;
    
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

        [Option("ConnectionType", 
            HelpText = "Required if you want to override the CdsConnection details in manifest")]
        public CdsConnectionType ConnectionType
        {
            set
            {
                CdsConnectionDetails.ConnectionType = value;
                OverrideManifestConnectionDetails = true;
            }
        }

        [Option("Url")]
        public string Url
        {
            set => CdsConnectionDetails.CdsUrl = value;
        }
        
        [Option("AppId")]
        public string AppId
        {
            set => CdsConnectionDetails.CdsAppId = value;
        }
        
        [Option("AppSecret")]
        public string AppSecret
        {
            set => CdsConnectionDetails.CdsAppSecret = value;
        }
        
        [Option("UserName")]
        public string UserName
        {
            set => CdsConnectionDetails.CdsUserName = value;
        }
        
        [Option("Password")]
        public string UserPassword
        {
            set => CdsConnectionDetails.CdsUserName = value;
        }
        
        [Option("ConnectionString")]
        public string ConnectionString
        {
            set => CdsConnectionDetails.CdsConnectionString = value;
        }
    }
}