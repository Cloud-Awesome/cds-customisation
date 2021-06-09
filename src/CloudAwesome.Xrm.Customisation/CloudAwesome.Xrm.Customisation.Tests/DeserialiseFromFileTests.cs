using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests
{
    [TestFixture]
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
        
    }
}
