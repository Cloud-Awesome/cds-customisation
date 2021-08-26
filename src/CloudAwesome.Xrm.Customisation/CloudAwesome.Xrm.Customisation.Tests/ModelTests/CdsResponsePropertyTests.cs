using System.Collections.Generic;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.ModelTests
{
    
    public class CdsResponsePropertyTests: BaseFakeXrmTest
    {
        [Test]
        [Description("A non-existent CustomAPI should create new")]
        [Ignore("WIP")]
        public void NewResponsePropertyShouldCreateNewRecord()
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
        public void ExistingResponsePropertyShouldUpdateRecord()
        {

        }

        [Test]
        [Description("...")]
        [Ignore("WIP")]
        public void RegisterNewResponsePropertyWorksWithMinimalDataSet()
        {

        }

    }
}