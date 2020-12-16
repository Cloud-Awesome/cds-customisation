using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.TracingWrapperTests
{
    [TestFixture]
    public class DeserialiseFromFileTests
    {
        [Test]
        public void DeserialisePluginManifest()
        {
            var manifestPath = "../../../CloudAwesome.Xrm.Customisation/SampleSchema/plugin-manifest.xml";

            var manifest = SerialisationWrapper.DeserialiseFromFile<PluginManifest>(manifestPath);

            Assert.AreEqual(1, manifest.PluginAssemblies.Length);
            Assert.AreEqual(2, manifest.PluginAssemblies[0].Plugins.Length);
        }
    }
}
