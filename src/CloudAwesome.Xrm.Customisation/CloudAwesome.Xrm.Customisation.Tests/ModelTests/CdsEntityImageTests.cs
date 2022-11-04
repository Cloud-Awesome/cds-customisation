using System.Collections.Generic;
using System.Linq;
using CloudAwesome.Xrm.Customisation.EarlyBoundModels;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.ModelTests
{
    public class CdsEntityImageTests: BaseFakeXrmTest
    {
        [Test]
        [Description("Given image does not exist, Register should create new")]
        public void RegisteringNonExistentImageShouldCreateNew()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>()
            {
                ExistingPluginStep
            });

            UnitTestImage.Register(orgService, ExistingPluginStep.ToEntityReference());

            var registeredImage =
                (from e in context.CreateQuery<SdkMessageProcessingStepImage>()
                    where e.Name == ExistingImage.Name
                    select e).ToList();

            Assert.AreEqual(1, registeredImage.Count);
            Assert.IsNotNull(registeredImage[0].Id);
        }

        [Test]
        [Description("Given existing image, Register should update")]
        public void RegisteringExistentImageShouldUpdate()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            ExistingImage.Attributes1 = "one,two,three,four";
            context.Initialize(new List<Entity>()
            {
                ExistingPluginStep,
                ExistingImage
            });

            UnitTestImage.Register(orgService, ExistingPluginStep.ToEntityReference());

            var registeredImage =
                (from e in context.CreateQuery<SdkMessageProcessingStepImage>()
                    where e.Name == ExistingImage.Name
                    select e).ToList();

            Assert.AreEqual(1, registeredImage.Count);
            Assert.AreEqual(ExistingImage.Id, registeredImage[0].Id);
            Assert.AreEqual("one,two,three", registeredImage[0].Attributes1);
        }
    }
}
