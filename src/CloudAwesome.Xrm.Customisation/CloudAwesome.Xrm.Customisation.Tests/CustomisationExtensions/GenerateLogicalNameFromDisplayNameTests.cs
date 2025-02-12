using FluentAssertions;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.CustomisationExtensions
{
    public class GenerateLogicalNameFromDisplayNameTests
    {
        [Test]
        [TestCase("This is a test", "awe", false, "awe_thisisatest")]
        [TestCase("This-is-0-test!!", "awe", false, "awe_thisis0test")]
        [TestCase("Test Laptop", "awe", true, "awe_testlaptopid")]
        [TestCase("Already An ID", "awe", false, "awe_alreadyanid")]
        [TestCase("A valid string with no Prefix?", "", false, "avalidstringwithnoprefix")]
        public void HappyPath(string displayName, string publisherPrefix, bool isLookup, string expectedOutput)
        {
            var logicalName = displayName.GenerateLogicalNameFromDisplayName(publisherPrefix, isLookup);
            logicalName.Should().Be(expectedOutput);
        }
    }
}
