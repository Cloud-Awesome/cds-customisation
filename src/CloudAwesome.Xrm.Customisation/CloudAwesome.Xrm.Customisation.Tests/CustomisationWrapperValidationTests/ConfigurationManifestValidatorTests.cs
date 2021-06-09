using FluentAssertions;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.CustomisationWrapperValidationTests
{
    [TestFixture]
    public class ConfigurationManifestValidatorTests
    {
        [Test]
        public void Invalid_manifest_should_throw_error_if_solution_name_is_not_populated()
        {
            var manifest = new ConfigurationManifest();
            var validator = new ConfigurationManifestValidator();
            var result = validator.Validate(manifest);

            result.IsValid.Should().Be(false, "solution name is mandatory");
        }

        [Test]
        public void Valid_manifest_should_return_valid_result()
        {
            var manifest = new ConfigurationManifest()
            {
                SolutionName = "TestSolution1"
            };
            var validator = new ConfigurationManifestValidator();
            var result = validator.Validate(manifest);

            result.IsValid.Should().Be(true, "solution name is only mandatory field");
        }
        
    }
}