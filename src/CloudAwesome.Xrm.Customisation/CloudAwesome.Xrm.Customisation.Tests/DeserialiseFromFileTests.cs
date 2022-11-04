using CloudAwesome.Xrm.Customisation.PluginRegistration;
using FluentAssertions;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests
{
    public class DeserialiseFromFileTests: BaseFakeXrmTest
    {
        [Test]
        public void DeserialisePluginManifest()
        {
            var manifestPath = $"{PluginManifestFolderPath}/plugin-manifest.xml";
            var manifest = SerialisationWrapper.DeserialiseFromFile<PluginManifest>(manifestPath);

            Assert.AreEqual(1, manifest.PluginAssemblies.Length);
            Assert.AreEqual(2, manifest.PluginAssemblies[0].Plugins.Length);
        }

        [Test]
        public void DeserialiseGenericJsonManifest()
        {
            var manifestPath = $"{TestManifestFolderPath}/SampleGenericManifest.json";
            var manifest = SerialisationWrapper.DeserialiseJsonFromFile<TestJsonForSerialisation>(manifestPath);

            manifest.FirstName.Should().Be("Arthur");
            manifest.LastName.Should().Be("Nicholson-Gumula");
        }
        
    }

    public class TestJsonForSerialisation
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
