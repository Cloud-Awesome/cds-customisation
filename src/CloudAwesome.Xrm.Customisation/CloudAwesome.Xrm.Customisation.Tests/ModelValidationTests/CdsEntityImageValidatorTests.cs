using CloudAwesome.Xrm.Customisation.Models;
using CloudAwesome.Xrm.Customisation.ModelValidators;
using FluentAssertions;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.ModelValidationTests
{
    public class CdsEntityImageValidatorTests
    {
        [Test]
        public void Missing_Mandatory_Fields_Should_Return_Errors()
        {
            var image = new CdsEntityImage();
            var validator = new CdsEntityImageValidator();

            var result = validator.Validate(image);

            result.IsValid.Should().BeFalse("mandatory data has not been provided");
            result.Errors.Should().HaveCount(2, "all three validators should fail");
        }

        [Test]
        public void Validation_Should_Fail_If_No_Attributes_Are_Provided()
        {
            var image = new CdsEntityImage()
            {
                Name = "Test",
                Type = EntityImageType.PreImage,
                Attributes = new string[]{}
            };
            var validator = new CdsEntityImageValidator();

            var result = validator.Validate(image);

            result.IsValid.Should().BeFalse("no attributes have been provided");
        }

        [Test]
        public void Valid_Entity_Image_Should_Pass_Validation()
        {
            var image = new CdsEntityImage()
            {
                Name = "Test",
                Attributes = new string[]
                {
                    "one",
                    "two"
                }
            };
            var validator = new CdsEntityImageValidator();

            var result = validator.Validate(image);

            result.IsValid.Should().BeTrue("all mandatory information has been provided");
        }
    }
}