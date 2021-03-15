using CloudAwesome.Xrm.Customisation.Models;
using FluentAssertions;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.PluginWrapperTests
{
    [TestFixture]
    public class ValidatePluginManifestTests
    {
        [Test]
        public void Valid_Plugin_Manifest_Returns_Positive_Result()
        {
            var manifest = new PluginManifest();
            var wrapper = new PluginWrapper();

            var result = wrapper.Validate(manifest);

            result.IsValid.Should().BeTrue("an empty manifest is valid");
        }

        [Test]
        public void Invalid_Manifest_Returns_Negative_Result_And_List_Of_Errors()
        {
            var manifest = new PluginManifest()
            {
                PluginAssemblies = new[]
                {
                    new CdsPluginAssembly()
                    {
                        Name = "Test Assembly",
                        Plugins = new[]
                        {
                            new CdsPlugin()
                            {
                                Name = "Test Plugin"
                            }
                        }
                    }
                }
            };
            var wrapper = new PluginWrapper();

            var result = wrapper.Validate(manifest);
            
            result.IsValid.Should().BeFalse("assembly and child plugin have missing mandatory data");
            result.Errors.Should().HaveCount(3, "assembly has two errors and child plugin has one");
        }
    }
}