using System;
using System.Collections.Generic;
using System.Linq;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.ModelTests
{
    [TestFixture]
    public class CdsPluginTests: BaseFakeXrmTest
    {
        [Test]
        [Description("Given an existing Assembly and Plugin, Plugin should be updated")]
        public void RegisterExistingPluginShouldUpdate()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>()
            {
                ExistingPluginAssembly,
                ExistingContactPluginType,
            });

            UnitTestPlugin.Register(orgService, ExistingPluginAssembly.ToEntityReference());

            var registeredPlugin =
                (from p in context.CreateQuery<PluginType>()
                    where p.Name == UnitTestPlugin.Name
                    select p).ToList();

            Assert.AreEqual(1, registeredPlugin.Count);
            Assert.AreEqual(registeredPlugin[0].Id, ExistingContactPluginType.Id);
        }

        [Test]
        [Description("Given an existing Assembly and new Plugin, Plugin should be created")]
        public void RegisterNewPluginShouldBeCreated()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>()
            {
                ExistingPluginAssembly
            });

            UnitTestPlugin.Register(orgService, ExistingPluginAssembly.ToEntityReference());

            var registeredPlugin =
                (from p in context.CreateQuery<PluginType>()
                    where p.Name == UnitTestPlugin.Name
                    select p).ToList();

            Assert.AreEqual(1, registeredPlugin.Count);
            Assert.IsNotNull(registeredPlugin[0].Id);
        }

        [Test]
        [Description("Given Register is passed an EntityReference which isn't of type PluginAssembly, should throw an exception")]
        public void RegisterShouldThrowExceptionIfIncorrectEntityReferenceIsPassedThrough()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            Assert.Throws<ArgumentException>(
                () => UnitTestPlugin.Register(orgService, new SdkMessageProcessingStep().ToEntityReference()));
        }

        [Test]
        [Description("Given an existing Plugin with child steps, should unregister children beforehand")]
        [Ignore("Method not implement. May be redundant")]
        public void UnregisteringPluginWithDependenciesShouldAlsoUnregisterChildSteps()
        {
            throw new NotImplementedException("Issue #37");
        }

        [Test]
        [Description("Given an existing Assembly and Plugin, Plugin should be removed")]
        [Ignore("Method not implement. May be redundant")]
        public void UnregisterPluginShouldRemoveExistingPlugin()
        {
            throw new NotImplementedException("Issue #37");
        }

        [Test]
        [Description("Given an existing Assembly and non-existent Plugin, something should happen...")]
        [Ignore("Method not implement. May be redundant")]
        public void UnregisterNonExistentPluginShouldDoSomething()
        {
            throw new NotImplementedException("Issue #37");
        }
    }
}