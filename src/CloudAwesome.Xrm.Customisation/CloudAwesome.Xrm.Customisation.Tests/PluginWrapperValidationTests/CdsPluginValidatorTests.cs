using CloudAwesome.Xrm.Customisation.Models;
using CloudAwesome.Xrm.Customisation.Validators;
using FluentAssertions;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.PluginWrapperValidationTests
{
    
    public class CdsPluginValidatorTests
    {
        [Test]
        public void Plugin_With_Missing_Mandatory_Data_Should_Have_Errors()
        {
            var plugin = new CdsPlugin();
            var validator = new CdsPluginValidator();

            var result = validator.Validate(plugin);

            result.IsValid.Should().BeFalse("mandatory data has not been provided");
            result.Errors.Should().HaveCount(2, "all mandatory fields are missing");
        }

        [Test]
        public void Plugin_With_Child_Steps_Should_Trigger_Child_Validation()
        {
            var plugin = new CdsPlugin()
            {
                Name = "Test",
                FriendlyName = "Friendly Test",
                Steps = new CdsPluginStep[]
                {
                    new CdsPluginStep()
                    {
                        Name = "Step Name",
                        FriendlyName = "Friendly Step Name"
                    }
                }
            };
            var validator = new CdsPluginValidator();

            var result = validator.Validate(plugin);

            result.IsValid.Should().BeFalse("child plugin step should fail validation");
        }
    }
}