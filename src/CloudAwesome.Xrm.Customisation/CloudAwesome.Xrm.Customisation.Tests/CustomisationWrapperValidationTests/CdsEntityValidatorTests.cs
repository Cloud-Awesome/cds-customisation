using CloudAwesome.Xrm.Customisation.Models;
using CloudAwesome.Xrm.Customisation.Validators;
using FluentAssertions;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.CustomisationWrapperValidationTests
{
    public class CdsEntityValidatorTests
    {
        [Test]
        public void Cds_entity_must_include_at_least_display_name()
        {
            var contact = new CdsEntity();
            var validator = new CdsEntityValidator();
            var result = validator.Validate(contact);

            result.IsValid.Should().Be(false, "display name is mandatory");
        }
        
        [Test]
        public void Invalid_entity_should_return_false()
        {
            var contact = new CdsEntity()
            {
                DisplayName = "Contact"
            };
            var validator = new CdsEntityValidator();
            var result = validator.Validate(contact);

            result.IsValid.Should().Be(true);
        }
    }
}