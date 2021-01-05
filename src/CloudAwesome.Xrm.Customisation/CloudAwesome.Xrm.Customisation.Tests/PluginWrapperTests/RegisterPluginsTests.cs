using System.Collections.Generic;
using System.Linq;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.PluginWrapperTests
{
    // TODO - other tests to vary between create and update
    // TODO - tests for tracing

    [TestFixture]
    public class RegisterPluginsTests: BaseFakeXrmTest
    {
        [Test]
        [Description("Plugin assembly and steps don't already exist and are registered successfully")]
        [Category("IgnoreInPipeline")]
        public void AssemblyDoesNotAlreadyExist()
        {
            var manifestPath = $"{TestManifestFolderPath}/plugin-manifest.xml";
            var manifest = SerialisationWrapper.DeserialiseFromFile<PluginManifest>(manifestPath);

            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();
            context.Initialize(new List<Entity>()
            {
                UpdateSdkMessage,
                CreateSdkMessage,
                UpdateContactMessageFilter,
                UpdateAccountMessageFilter,
                CreateContactMessageFilter
            });

            var pluginWrapper = new PluginWrapper();
            pluginWrapper.RegisterPlugins(manifest, orgService);

            var postRegisteredAssemblies = 
                (from a in context.CreateQuery<PluginAssembly>()
                 where a.Name == "SamplePluginAssembly" 
                 select a).ToList();

            Assert.AreEqual(1, postRegisteredAssemblies.Count);

        }

        [Test]
        [Description("Plugin assembly exists with no steps and are registered/updated successfully")]
        [Category("IgnoreInPipeline")]
        public void AssemblyExistsWithNoRegisteredPlugins()
        {
            var manifestPath = $"{TestManifestFolderPath}/plugin-manifest.xml";
            var manifest = SerialisationWrapper.DeserialiseFromFile<PluginManifest>(manifestPath);

            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();
            context.Initialize(new List<Entity>()
            {
                UpdateSdkMessage,
                CreateSdkMessage,
                UpdateContactMessageFilter,
                UpdateAccountMessageFilter,
                CreateContactMessageFilter
            });
            
            var pluginWrapper = new PluginWrapper();
            pluginWrapper.RegisterPlugins(manifest, orgService);

            var postRegisteredPluginTypes =
                (from p in context.CreateQuery<PluginType>()
                    select p).ToList();

            Assert.AreEqual(2, postRegisteredPluginTypes.Count);

        }

        [Test]
        [Description("Plugin assembly exists with one existing step. New step is added")]
        [Category("IgnoreInPipeline")]
        public void RemovesPluginTypeFromExistingAssembly()
        {
            var manifestPath = $"{TestManifestFolderPath}/plugin-manifest.xml";
            var manifest = SerialisationWrapper.DeserialiseFromFile<PluginManifest>(manifestPath);

            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();
            context.Initialize(new List<Entity>()
            {
                SamplePluginAssembly,
                UpdateContact,
                PluginTypeToBeRemoved,
                UpdateSdkMessage,
                CreateSdkMessage,
                UpdateContactMessageFilter,
                UpdateAccountMessageFilter,
                CreateContactMessageFilter
            });

            var pluginWrapper = new PluginWrapper();
            pluginWrapper.RegisterPlugins(manifest, orgService);

            var postRegisteredPluginAssembly =
                (from a in context.CreateQuery<PluginAssembly>()
                    where Equals(a.Name, SamplePluginAssembly.Name) 
                          && Equals(a.Version, SamplePluginAssembly.Version)
                    select a).FirstOrDefault();

            var postRegisteredPluginTypes =
                (from p in context.CreateQuery<PluginType>()
                    where Equals(p.PluginAssemblyId, postRegisteredPluginAssembly.ToEntityReference())
                    select p).ToList();

            Assert.AreEqual(2, postRegisteredPluginTypes.Count);
            Assert.IsFalse(postRegisteredPluginTypes.Contains(PluginTypeToBeRemoved));

        }

    }
}
