using System;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using CloudAwesome.Xrm.Core.Models;
using CloudAwesome.Xrm.Customisation.Models;
using CloudAwesome.Xrm.Customisation.PluginRegistration;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation.Tests
{
    public class BaseFakeXrmTest
    {
        public const string PluginManifestFolderPath = "../../TestManifests/PluginRegistration";
        public const string PortalManifestFolderPath = "../../TestManifests/PortalConfiguration";

        #region Query definitions and test data
        // TODO - rearrange/organise the test objects...

        public static readonly CdsConnection MockCdsConnection = new CdsConnection()
        {
            ConnectionType = CdsConnectionType.AppRegistration,
            CdsAppSecret = "ThisIsASecret",
            CdsAppId = "--This_Is_A_Sample_Secret--"
        };

        public static readonly PluginManifest InvalidPluginManifest = new PluginManifest()
        {
            CdsConnection = MockCdsConnection,
            PluginAssemblies = new CdsPluginAssembly[]
            {
                new CdsPluginAssembly()
            }
        };
        
        public static readonly PluginManifest SampleFullPluginManifest = new PluginManifest()
        {
            CdsConnection = MockCdsConnection,
            PluginAssemblies = new CdsPluginAssembly[]
            {
                new CdsPluginAssembly()
                {
                    Name = "SamplePluginAssembly",
                    FriendlyName = "Account and Contact Plugins",
                    Assembly = "../../../SamplePluginAssembly/bin/release/SamplePluginAssembly.dll",
                    // TODO - ^^ This is a test smell and should mock the file system instead of relying on a real dll =/
                    Plugins = new CdsPlugin[]
                    {
                        new CdsPlugin()
                        {
                            Name = "SamplePluginAssembly.ContactPlugin",
                            FriendlyName = "Contact Plugin",
                            Steps = new CdsPluginStep[]
                            {
                                new CdsPluginStep()
                                {
                                    Name = "Update Contact: On Update of Contact",
                                    FriendlyName = "Update Contact: On Update of Contact",
                                    Stage = SdkMessageProcessingStep_Stage.Postoperation,
                                    ExecutionMode = SdkMessageProcessingStep_Mode.Synchronous,
                                    Message = "update",
                                    PrimaryEntity = "contact"
                                }
                            }
                        },
                        new CdsPlugin()
                        {
                            Name = "SamplePluginAssembly.AccountPlugin",
                            FriendlyName = "Account Plugin",
                            Steps = new CdsPluginStep[]
                            {
                                new CdsPluginStep()
                                {
                                    Name = "Account Plugin: On Update of an Account",
                                    FriendlyName = "Account Plugin: On Update of an Account",
                                    Stage = SdkMessageProcessingStep_Stage.Postoperation,
                                    ExecutionMode = SdkMessageProcessingStep_Mode.Synchronous,
                                    Message = "update",
                                    PrimaryEntity = "account"
                                }
                            }
                        }
                    }
                }
            },
            ServiceEndpoints = new CdsServiceEndpoint[]{}
        };
        
        public static readonly PluginManifest SampleCustomApiManifest = new PluginManifest()
        {
            CdsConnection = MockCdsConnection,
            PluginAssemblies = new CdsPluginAssembly[]
            {
                new CdsPluginAssembly()
                {
                    Name = "SamplePluginAssembly",
                    FriendlyName = "Account and Contact Plugins",
                    Assembly = "../../../SamplePluginAssembly/bin/release/SamplePluginAssembly.dll",
                    // TODO - ^^ This is a test smell and should mock the file system instead of relying on a real dll =/
                    Plugins = new CdsPlugin[]
                    {
                        new CdsPlugin()
                        {
                            Name = "SamplePluginAssembly.ContactPlugin",
                            FriendlyName = "Contact Plugin",
                            CustomApis = new CdsCustomApi[]
                            {
                                new CdsCustomApi()
                                {
                                    Name = "ContactCreationApi",
                                    FriendlyName = "Contact Creation API",
                                    IsFunction = false,
                                    AllowedCustomProcessingStepType = CustomAPI_AllowedCustomProcessingStepType.AsyncOnly,
                                    BindingType = CustomAPI_BindingType.Global,
                                    Description = "This does something on creation of a Contact......",
                                    RequestParameters = new CdsRequestParameter[]
                                    {
                                        new CdsRequestParameter()
                                        {
                                            Name = "First name",
                                            Description = "What is the contact's first name"
                                        },
                                        new CdsRequestParameter()
                                        {
                                            Name = "Last name",
                                            Description = "What is the contact's last name"
                                        }
                                    },
                                    ResponseProperties = new CdsResponseProperty[]
                                    {
                                        new CdsResponseProperty()
                                        {
                                            Name = "Full name",
                                            Type = CustomAPIFieldType.String
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            },
            ServiceEndpoints = new CdsServiceEndpoint[]{}
        };
        
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
            Name = "UnitTestAssembly",
            Version = "1.0.0.0",
            Id = Guid.NewGuid()
        };

        public static readonly PluginType ExistingContactPluginType = new PluginType()
        {
            PluginAssemblyId = ExistingPluginAssembly.ToEntityReference(),
            Name = "ContactPlugin",
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

        public static readonly CdsPlugin UnitTestPlugin = new CdsPlugin()
        {
            Name = "ContactPlugin",
            Description = "This is the plugin related to the Contact entity",
            FriendlyName = " Contact Plugins"
        };

        public static readonly CdsPluginStep UnitTestPluginStep = new CdsPluginStep()
        {
            Name = "OnContactUpdate",
            Description = "This is triggered on update of a contact",
            ExecutionMode = SdkMessageProcessingStep_Mode.Synchronous,
            Message = "update",
            Stage = SdkMessageProcessingStep_Stage.Postoperation,
        };

        public static readonly CdsEntityImage UnitTestImage = new CdsEntityImage()
        {
            Name = "ContactImage",
            Type = EntityImageType.PreImage,
            Attributes = new[] {"one", "two", "three"}
        };

        #endregion Query definitions and test data
    }
}
