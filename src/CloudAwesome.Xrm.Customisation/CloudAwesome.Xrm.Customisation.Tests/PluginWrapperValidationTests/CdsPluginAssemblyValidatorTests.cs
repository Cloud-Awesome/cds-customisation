using CloudAwesome.Xrm.Customisation.Models;
using CloudAwesome.Xrm.Customisation.Validators;
using FluentAssertions;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.PluginWrapperValidationTests
{
    
    public class CdsPluginAssemblyValidatorTests
    {
        [Test]
        public void Mandatory_Assembly_Registration_Information_Is_Entered_Returns_Positive_Result()
        {
            var pluginAssembly = new CdsPluginAssembly()
            {
                Name = "TestPluginAssembly",
                FriendlyName = "Test Plugin Assembly",
                Assembly = "c:/FakePath/Assembly.dll"
            };

            var validator = new CdsPluginAssemblyValidator();
            var result = validator.Validate(pluginAssembly);

            result.IsValid.Should().BeTrue("all mandatory fields are populated");
        }
        
        [Test]
        public void Mandatory_Assembly_Registration_Information_Is_Not_Entered_Returns_Negative_Result()
        {
            var pluginAssembly = new CdsPluginAssembly() { };

            var validator = new CdsPluginAssemblyValidator();
            var result = validator.Validate(pluginAssembly);

            result.IsValid.Should().BeFalse("mandatory fields are not populated");
            result.Errors.Should().NotBeEmpty("because error details should be included").And
                .HaveCount(3, "three missing mandatory fields are missing");
        }

        [Test]
        public void Assembly_With_Child_Plugins_Should_Trigger_Child_Validation()
        {
            var assembly = new CdsPluginAssembly()
            {
                Name = "TestPluginAssembly",
                FriendlyName = "Test Plugin Assembly",
                Assembly = "c:/FakePath/Assembly.dll",
                Plugins = new CdsPlugin[]
                {
                    new CdsPlugin()
                    {
                        Name = "Test"
                    }
                }
            };
            var validator = new CdsPluginAssemblyValidator();

            var result = validator.Validate(assembly);

            result.IsValid.Should().BeFalse("child plugin has validation errors");
        }
    }
}