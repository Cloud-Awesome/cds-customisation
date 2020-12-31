using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;

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

        public static readonly SdkMessage CreateSdkMessage = new SdkMessage()
        {
            Id = Guid.NewGuid(),
            Name = "create"
        };

        public static readonly Entity UpdateContactMessageFilter = new Entity()
        {
            LogicalName = SdkMessageFilter.EntityLogicalName,
            Id = Guid.NewGuid(),
            Attributes = new AttributeCollection()
            {
                { SdkMessageFilter.Fields.SdkMessageId, UpdateSdkMessage.Id },
                { SdkMessageFilter.Fields.PrimaryObjectTypeCode, "contact" }
            }
        };

        public static readonly Entity UpdateAccountMessageFilter = new Entity()
        {
            LogicalName = SdkMessageFilter.EntityLogicalName,
            Id = Guid.NewGuid(),
            Attributes = new AttributeCollection()
            {
                { SdkMessageFilter.Fields.SdkMessageId, UpdateSdkMessage.Id },
                { SdkMessageFilter.Fields.PrimaryObjectTypeCode, "account" }
            }
        };

        public static readonly Entity CreateAccountMessageFilter = new Entity()
        {
            LogicalName = SdkMessageFilter.EntityLogicalName,
            Id = Guid.NewGuid(),
            Attributes = new AttributeCollection()
            {
                { SdkMessageFilter.Fields.SdkMessageId, CreateSdkMessage.Id },
                { SdkMessageFilter.Fields.PrimaryObjectTypeCode, "account" }
            }
        };

        public static readonly Entity CreateContactMessageFilter = new Entity()
        {
            LogicalName = SdkMessageFilter.EntityLogicalName,
            Id = Guid.NewGuid(),
            Attributes = new AttributeCollection()
            {
                { SdkMessageFilter.Fields.SdkMessageId, CreateSdkMessage.Id },
                { SdkMessageFilter.Fields.PrimaryObjectTypeCode, "contact" }
            }
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

        public static readonly ServiceEndpoint ExistingTesterQueueEndpoint = new ServiceEndpoint()
        {
            Id = Guid.NewGuid(),
            Name = "TesterQueue"
        };

        #endregion Query definitions and test data
    }
}
