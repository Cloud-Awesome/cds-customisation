using CloudAwesome.Xrm.Core.Models;
using CloudAwesome.Xrm.Customisation.Models;
using CloudAwesome.Xrm.Customisation.PluginRegistration;
using FluentAssertions;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.PluginRegistrationTests
{
    
    public class PluginManifestValidatorTests
    {
        [Test]
        public void Empty_Manifest_Should_Throw_No_Errors()
        {
            var manifest = new PluginManifest()
            {
                CdsConnection = new CdsConnection()
                {
                    CdsUrl = "https://testurl.crm.dynamics.com",
                    CdsAppId = "ID",
                    CdsAppSecret = "SECRET!"
                }
            };
            var validator = new PluginManifestValidator();
            var result = validator.Validate(manifest);

            result.IsValid.Should().BeTrue("only CDS connection is mandatory in the manifest");
        }

        [Test]
        public void Manifest_Should_Call_Child_Validators_If_Populated_And_Bubble_Up_Errors()
        {
            var manifest = new PluginManifest()
            {
                CdsConnection = new CdsConnection()
                {
                    CdsUrl = "https://testurl.crm.dynamics.com",
                    CdsAppId = "ID",
                    CdsAppSecret = "SECRET!"
                },
                PluginAssemblies = new CdsPluginAssembly[]
                {
                    new CdsPluginAssembly()
                    {
                        Name = "Test",
                        FriendlyName = "Friend Test"
                    }
                }
            };
            var validator = new PluginManifestValidator();
            var result = validator.Validate(manifest);

            result.IsValid.Should().BeFalse("Child plugin assembly has missing mandatory data");
        }
        
        [Test]
        public void Manifest_Should_Call_GrandChild_Validators_If_Populated_And_Bubble_Up_Errors()
        {
            var manifest = new PluginManifest()
            {
                CdsConnection = new CdsConnection()
                {
                    CdsUrl = "https://testurl.crm.dynamics.com",
                    CdsAppId = "ID",
                    CdsAppSecret = "SECRET!"
                },
                PluginAssemblies = new CdsPluginAssembly[]
                {
                    new CdsPluginAssembly()
                    {
                        Name = "Test",
                        FriendlyName = "Friend Test",
                        Assembly = "c:/fake/test.dll",
                        Plugins = new CdsPlugin[]
                        {
                            new CdsPlugin()
                            {
                                Name = "Test Plugin"
                            }
                        }
                    }
                }
            };
            var validator = new PluginManifestValidator();
            var result = validator.Validate(manifest);

            result.IsValid.Should().BeFalse("grandchild plugin has missing mandatory data");
        }
    }
}