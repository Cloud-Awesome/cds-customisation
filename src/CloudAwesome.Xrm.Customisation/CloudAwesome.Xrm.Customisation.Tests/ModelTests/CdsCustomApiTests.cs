using System.Collections.Generic;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.ModelTests
{
    public class CdsCustomApiTests: BaseFakeXrmTest
    {
        [Test]
        [Description("A non-existent CustomAPI should create new")]
        [Ignore("WIP")]
        public void NewCustomApiShouldCreateNewRecord()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>()
            {
                ExistingPluginStep
            });

            // ...
        }

        [Test]
        [Description("...")]
        [Ignore("WIP")]
        public void ExistingCustomApiShouldUpdateRecord()
        {

        }

        [Test]
        [Description("...")]
        [Ignore("WIP")]
        public void RegisterNewCustomApiWorksWithMinimalDataSet()
        {

        }

        [Test]
        [Description("...")]
        [Ignore("Method not written yet!")]
        public void UnregisterCustomApiShouldRemoveExistingRecordAndChildRecords()
        {

        }

        [Test]
        [Description("...")]
        [Ignore("Method not written yet!")]
        public void UnregisterCustomApiThatDoesntExistShouldDoSomething()
        {
            
        }
    }
}