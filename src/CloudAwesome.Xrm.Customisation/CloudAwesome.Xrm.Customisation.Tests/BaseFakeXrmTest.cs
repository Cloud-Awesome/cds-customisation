using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudAwesome.Xrm.Customisation.Tests
{
    public class BaseFakeXrmTest
    {
        public const string TestManifestFolderPath = "../../TestManifests/PluginRegistration";

        #region Query definitions and test data

        public static readonly SdkMessage UpdateSdkMessage = new SdkMessage()
        {
            Id = Guid.NewGuid(),
            Name = "update"
        };

        public static readonly PluginAssembly SamplePluginAssembly = new PluginAssembly()
        {
            Id = Guid.NewGuid(),
            Name = "SamplePluginAssembly",
            Version = "1.0.0.0"
        };

        public static readonly PluginType UpdateContact = new PluginType()
        {
            Id = Guid.NewGuid(),
            Name = "SamplePluginAssembly.UpdateContact",
            PluginAssemblyId = SamplePluginAssembly.ToEntityReference()
        };

        public static readonly PluginType PluginTypeToBeRemoved = new PluginType()
        {
            Id = Guid.NewGuid(),
            Name = "SamplePluginAssembly.RemoveMe",
            PluginAssemblyId = SamplePluginAssembly.ToEntityReference()
        };

        #endregion Query definitions and test data
    }
}
