using System.Collections.Generic;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.ModelTests
{
    
    public class CdsRequestParameterTests: BaseFakeXrmTest
    {
        [Test]
        [Description("A non-existent CustomAPI should create new")]
        [Ignore("WIP")]
        public void NewRequestParameterShouldCreateNewRecord()
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
        public void ExistingRequestParameterShouldUpdateRecord()
        {

        }

        [Test]
        [Description("...")]
        [Ignore("WIP")]
        public void RegisterRequestParameterWorksWithMinimalDataSet()
        {

        }
        
        [Test]
        [Description("If the Custom API is a Function, no child Request Parameters can be of type Entity")]
        [Ignore("WIP")]
        public void FunctionApiCannotHaveRequestParameterOrTypeEntity()
        {
            // TODO - for future enhancements, this functionality should also be done as validation before processing manifest

        }

        [Test]
        [Description("If the Custom API is a Function, no child Request Parameters can be of type Entity Collection")]
        [Ignore("WIP")]
        public void FunctionApiCannotHaveRequestParameterOrTypeEntityCollection()
        {

        }
    }
}