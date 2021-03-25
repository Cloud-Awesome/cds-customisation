using System.Collections.Generic;
using System.Linq;
using CloudAwesome.Xrm.Customisation.Models;
using FakeXrmEasy;
using FluentAssertions;
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
        public void New_Plugin_Assembly_Ans_Steps_Should_Be_Registered()
        {
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
            pluginWrapper.RegisterPlugins(SampleFullPluginManifest, orgService);

            var postRegisteredAssemblies = 
                (from a in context.CreateQuery<PluginAssembly>()
                 where a.Name == "SamplePluginAssembly" 
                 select a).ToList();

            postRegisteredAssemblies.Should().HaveCount(1);

        }

        [Test]
        [Description("Plugin assembly exists with no steps and are registered/updated successfully")]
        public void Existing_Assembly_With_No_Plugins_Should_Update_Assembly_WithNew_Plugins()
        {
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
            pluginWrapper.RegisterPlugins(SampleFullPluginManifest, orgService);

            var postRegisteredPluginTypes =
                (from p in context.CreateQuery<PluginType>() 
                    select p).ToList();

            postRegisteredPluginTypes.Should().HaveCount(2);
        }

        [Test]
        public void Clobbering_Existing_Assembly_Removes_Deleted_Steps_And_Registers_New_Steps()
        {
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
            
            SampleFullPluginManifest.Clobber = true;

            var pluginWrapper = new PluginWrapper();
            pluginWrapper.RegisterPlugins(SampleFullPluginManifest, orgService);

            var postRegisteredPluginAssembly =
                (from a in context.CreateQuery<PluginAssembly>()
                    where Equals(a.Name, SamplePluginAssembly.Name) 
                          && Equals(a.Version, SamplePluginAssembly.Version)
                    select a).FirstOrDefault();

            var postRegisteredPluginTypes =
                (from p in context.CreateQuery<PluginType>()
                    where Equals(p.PluginAssemblyId, postRegisteredPluginAssembly.ToEntityReference())
                    select p).ToList();
            
            postRegisteredPluginTypes.Should().HaveCount(2);
            postRegisteredPluginTypes.Should().NotContain(PluginTypeToBeRemoved);
            
        }

    }
}
