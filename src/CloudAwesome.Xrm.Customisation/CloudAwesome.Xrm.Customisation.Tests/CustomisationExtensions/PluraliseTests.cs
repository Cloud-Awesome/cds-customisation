using FluentAssertions;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.CustomisationExtensions
{
    public class PluraliseTests
    {
        [Test]
        [TestCase("Laptop", "Laptops")]
        [TestCase("Hobits", "Hobitses")]
        [TestCase("Fox", "Foxes")]
        [TestCase("Story", "Stories")]
        [TestCase("Wolf", "Wolves")]
        [TestCase("", "")]
        [TestCase("Irregular Person", "Irregular Persons")]
        [TestCase("Irregular Mouse", "Irregular Mouses")]
        public void HappyPath(string inputString, string expectedOutput)
        {
            var actual = inputString.Pluralise();
            actual.Should().Be(expectedOutput);
        }
    }
}
