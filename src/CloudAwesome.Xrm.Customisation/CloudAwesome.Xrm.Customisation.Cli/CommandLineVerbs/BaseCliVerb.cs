using CloudAwesome.Xrm.Customisation.Models;

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
        public string Manifest { get; set; }
        
        public CdsConnectionType ConnectionType
        {
            set
            {
                CdsConnectionDetails.ConnectionType = value;
                OverrideManifestConnectionDetails = true;
            }
        }
        
        public string Url
        {
            set => CdsConnectionDetails.CdsUrl = value;
        }
        
        public string AppId
        {
            set => CdsConnectionDetails.CdsAppId = value;
        }
        
        public string AppSecret
        {
            set => CdsConnectionDetails.CdsAppSecret = value;
        }
        
        public string UserName
        {
            set => CdsConnectionDetails.CdsUserName = value;
        }
        
        public string UserPassword
        {
            set => CdsConnectionDetails.CdsUserName = value;
        }
        
        public string ConnectionString
        {
            set => CdsConnectionDetails.CdsConnectionString = value;
        }
        
        public string FilePath { get; set; }
    }
}