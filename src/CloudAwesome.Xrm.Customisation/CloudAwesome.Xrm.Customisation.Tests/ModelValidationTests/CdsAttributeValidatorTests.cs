using System.Linq;
using CloudAwesome.Xrm.Customisation.Models;
using CloudAwesome.Xrm.Customisation.ModelValidators;
using FluentAssertions;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.ModelValidationTests
{
    [TestFixture]
    public class CdsAttributeValidatorTests
    {
        [Test]
        public void Display_Name_Throws_Exception()
        {
            var attribute = new CdsAttribute();
            var validator = new CdsAttributeValidator();

            var result = validator.Validate(attribute);

            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().BeGreaterThan(0);
            result.Errors.FirstOrDefault()?.ErrorMessage.Should()
                .Contain("Display name is always a mandatory field.");
        }

        [Test]
        public void Invalid_Memo_Type_Should_Through_Exception()
        {
            var attribute = new CdsAttribute
            {
                DisplayName = "TestMemo",
                DataType = CdsAttributeDataType.Memo
            };
            var validator = new CdsAttributeValidator();

            var result = validator.Validate(attribute);

            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().BeGreaterThan(0);
            result.Errors.FirstOrDefault()?.ErrorMessage.Should()
                .Contain("TestMemo: Max length is a mandatory field");

        }
    }
}