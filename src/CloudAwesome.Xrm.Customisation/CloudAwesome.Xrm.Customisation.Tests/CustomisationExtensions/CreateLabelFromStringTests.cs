using FluentAssertions;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.CustomisationExtensions
{
    public class CreateLabelFromStringTests
    {
        [Test]
        [TestCase("Laptop")]
        [TestCase("This is a description attribute")]
        public void HappyPath(string displayName)
        {
            var label = displayName.CreateLabelFromString();
            var expectedOutput = new Label(displayName, 1033);

            label.LocalizedLabels[0].Label.Should().Be(expectedOutput.LocalizedLabels[0].Label);
            label.LocalizedLabels[0].LanguageCode.Should().Be(expectedOutput.LocalizedLabels[0].LanguageCode);
        }

        [Test]
        [TestCase("Ceci n'est pas un pipe", 1036)]
        [TestCase("Bardzo lubię mój mąż", 1045)]
        public void NonEnglishLanguageCode(string displayName, int languageCode)
        {
            var label = displayName.CreateLabelFromString(languageCode);
            var expectedOutput = new Label(displayName, languageCode);

            label.LocalizedLabels[0].Label.Should().Be(expectedOutput.LocalizedLabels[0].Label);
            label.LocalizedLabels[0].LanguageCode.Should().Be(expectedOutput.LocalizedLabels[0].LanguageCode);
        }
    }
}
