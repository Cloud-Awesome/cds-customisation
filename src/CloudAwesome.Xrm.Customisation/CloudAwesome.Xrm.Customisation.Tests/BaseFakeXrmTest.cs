using System;
using System.IO;
using CloudAwesome.Xrm.Customisation.Models;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation.Tests
{
    public class BaseFakeXrmTest
    {
        public const string TestManifestFolderPath = "../../TestManifests/PluginRegistration";

        #region Query definitions and test data
        // TODO - rearrange/organise the test objects...

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

        public static readonly SdkMessageFilter UpdateContactMessageFilter = new SdkMessageFilter()
        {
            LogicalName = SdkMessageFilter.EntityLogicalName,
            Id = Guid.NewGuid(),
            Attributes = new AttributeCollection()
            {
                { SdkMessageFilter.Fields.SdkMessageId, UpdateSdkMessage.Id },
                { SdkMessageFilter.Fields.PrimaryObjectTypeCode, "contact" }
            }
        };

        public static readonly SdkMessageFilter UpdateAccountMessageFilter = new SdkMessageFilter()
        {
            LogicalName = SdkMessageFilter.EntityLogicalName,
            Id = Guid.NewGuid(),
            Attributes = new AttributeCollection()
            {
                { SdkMessageFilter.Fields.SdkMessageId, UpdateSdkMessage.Id },
                { SdkMessageFilter.Fields.PrimaryObjectTypeCode, "account" }
            }
        };

        public static readonly SdkMessageFilter CreateAccountMessageFilter = new SdkMessageFilter()
        {
            LogicalName = SdkMessageFilter.EntityLogicalName,
            Id = Guid.NewGuid(),
            Attributes = new AttributeCollection()
            {
                { SdkMessageFilter.Fields.SdkMessageId, CreateSdkMessage.Id },
                { SdkMessageFilter.Fields.PrimaryObjectTypeCode, "account" }
            }
        };

        public static readonly SdkMessageFilter CreateContactMessageFilter = new SdkMessageFilter()
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

        public static readonly PluginAssembly ExistingPluginAssembly = new PluginAssembly()
        {
            Name = "ExistingAssembly",
            Id = Guid.NewGuid()
        };

        public static readonly PluginType ExistingContactPluginType = new PluginType()
        {
            PluginAssemblyId = ExistingPluginAssembly.ToEntityReference(),
            Name = "ExistingContactPluginType",
            Id = Guid.NewGuid()
        };

        public static readonly SdkMessageProcessingStep ExistingPluginStep = new SdkMessageProcessingStep()
        {
            Id = Guid.NewGuid(),
            Name = "OnContactUpdate",
            Description = "This is triggered on update of a contact",
            Mode = SdkMessageProcessingStep_Mode.Synchronous,
            SdkMessageId = UpdateSdkMessage.ToEntityReference(),
            SdkMessageFilterId = UpdateContactMessageFilter.ToEntityReference(),
            Stage = SdkMessageProcessingStep_Stage.Postoperation,
            EventHandler = ExistingContactPluginType.ToEntityReference()
        };

        public static readonly SdkMessageProcessingStepImage ExistingImage = new SdkMessageProcessingStepImage()
        {
            SdkMessageProcessingStepId = ExistingPluginStep.ToEntityReference(),
            ImageType = SdkMessageProcessingStepImage_ImageType.PreImage,
            Name = "ContactImage",
            Id = Guid.NewGuid()
        };

        public static readonly CdsPluginStep Step = new CdsPluginStep()
        {
            Name = "OnContactUpdate",
            Description = "This is triggered on update of a contact",
            ExecutionMode = SdkMessageProcessingStep_Mode.Synchronous,
            Message = "update",
            Stage = SdkMessageProcessingStep_Stage.Postoperation,
        };

        public static readonly CdsEntityImage image = new CdsEntityImage()
        {
            Name = "ContactImage",
            Type = EntityImageType.PreImage,
            Attributes = new[] {"one", "two", "three"}
        };

        #endregion Query definitions and test data
    }
}
