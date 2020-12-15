using FakeXrmEasy;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.PluginWrapperTests
{
    [TestFixture]
    public class RegisterPluginsTests
    {
        [Test]
        [Ignore("Not ready - needs query results in faked context")]
        public void HappyPath()
        {
            var manifestPath = "../../../CloudAwesome.Xrm.Customisation/SampleSchema/plugin-manifest.xml";
            var manifest = SerialisationWrapper.DeserialiseFromFile<PluginManifest>(manifestPath);

            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();
            
            // Wednesday...
            // TODO - need to set up all the query results
            // TODO - other tests to vary between create and update
            // TODO - tests for tracing

            var pluginWrapper = new PluginWrapper();
            pluginWrapper.RegisterPlugins(manifest, orgService);
        }
    }
}
