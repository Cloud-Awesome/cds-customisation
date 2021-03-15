using CloudAwesome.Xrm.Customisation.Models;
using CloudAwesome.Xrm.Customisation.Validators;
using FluentAssertions;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.PluginWrapperValidationTests
{
    [TestFixture]
    public class CdsPluginStepValidatorTests
    {
        [Test]
        public void Step_With_Missing_Mandatory_Data_Should_Return_Errors()
        {
            var step = new CdsPluginStep();
            var validator = new CdsPluginStepValidator();

            var result = validator.Validate(step);
    
            result.IsValid.Should().BeFalse("mandatory fields are missing");
            result.Errors.Should().HaveCount(5, "all mandatory fields are missing");
        }

        [Test]
        public void Step_With_Child_Images_Should_Call_Child_Validator()
        {
            var step = new CdsPluginStep()
            {
                Name = "Test",
                FriendlyName = "Friendly Name",
                Stage = SdkMessageProcessingStep_Stage.Preoperation,
                Message = "create",
                PrimaryEntity = "contact",
                EntityImages = new CdsEntityImage[]
                {
                    new CdsEntityImage()
                }
            };
            var validator = new CdsPluginStepValidator();

            var result = validator.Validate(step);

            result.IsValid.Should().BeFalse("child entity image should fail validation");
        }
    }
}