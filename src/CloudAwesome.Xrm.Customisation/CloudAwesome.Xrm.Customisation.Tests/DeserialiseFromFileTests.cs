using System;
using System.IO;
using CloudAwesome.Xrm.Customisation.EarlyBoundModels;
using CloudAwesome.Xrm.Customisation.PluginRegistration;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using NJsonSchema;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests
{
    [TestFixture]
    public class DeserialiseFromFileTests: BaseFakeXrmTest
    {
        [Test]
        [Obsolete]
        public void DeserialisePluginManifest()
        {
            var manifestPath = $"{PluginManifestFolderPath}/plugin-manifest.xml";
            var manifest = SerialisationWrapper.DeserialiseFromFile<PluginManifest>(manifestPath);

            manifest.PluginAssemblies.Length.Should().Be(1);
            manifest.PluginAssemblies[0].Plugins.Length.Should().Be(2);
        }

        [Test]
        public void Deserialise_Json_Plugin_Manifest()
        {
            var manifestPath = "../../../CloudAwesome.Xrm.Customisation/SampleSchema/plugin-manifest.json";
            var manifest = SerialisationWrapper.DeserialiseJsonFromFile<PluginManifest>(manifestPath);

            Console.WriteLine(manifest.JsonSchema);
            
            manifest.Clobber.Should().Be(true);
            manifest.PluginAssemblies.Length.Should().Be(2);
            manifest.PluginAssemblies[0].Plugins[0]
                .Steps[0].ExecutionMode.Should().Be(SdkMessageProcessingStep_Mode.Asynchronous);
        }
        
        [Test]
        public void Deserialised_Json_Plugin_Manifest_Validates_Against_Schema()
        {
            var schemaPath = "../../../CloudAwesome.Xrm.Customisation/JsonSchema/plugin-manifest-schema.json";
            var schema = JsonSchema.FromFileAsync(schemaPath).Result;
            
            var manifestPath = "../../../CloudAwesome.Xrm.Customisation/SampleSchema/plugin-manifest.json";
            var text = File.ReadAllText(manifestPath);
            var json = JToken.Parse(text);

            var errors = schema.Validate(json);
            errors.Should().BeEmpty();
        }
        
    }
}
