﻿using System;
using System.Collections.Generic;
using System.Linq;
using CloudAwesome.Xrm.Customisation.Models;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.ModelTests
{
    [TestFixture]
    public class CdsPluginStepTests: BaseFakeXrmTest
    {
        [Test]
        [Description("Given an existing PluginType and Step, the Step should be updated")]
        public void RegisterExistentStepShouldUpdate()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>()
            {
                ExistingContactPluginType,
                UpdateSdkMessage,
                UpdateContactMessageFilter,
                ExistingPluginStep
            });

            Step.Register(orgService, ExistingContactPluginType.ToEntityReference(),
                UpdateSdkMessage.ToEntityReference(), UpdateContactMessageFilter.ToEntityReference());

            var registeredStep =
                (from e in context.CreateQuery<SdkMessageProcessingStep>()
                    where e.Name == Step.Name
                    select e).ToList();

            Assert.AreEqual(1, registeredStep.Count);
            Assert.AreEqual(registeredStep[0].Id, ExistingPluginStep.Id);
        }

        [Test]
        [Description("Given an existing PluginType, a new child Step should be registered")]
        public void RegisterNonExistentStepCreatesNewStep()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>()
            {
                ExistingContactPluginType,
                UpdateSdkMessage,
                UpdateContactMessageFilter
            });
            
            Step.Register(orgService, ExistingContactPluginType.ToEntityReference(), 
                UpdateSdkMessage.ToEntityReference(), UpdateContactMessageFilter.ToEntityReference());

            var registeredStep =
                (from e in context.CreateQuery<SdkMessageProcessingStep>()
                    where e.Name == Step.Name
                    select e).ToList();

            Assert.AreEqual(1, registeredStep.Count);
            Assert.IsNotNull(registeredStep[0].Id);
        }

        [Test]
        [Description("Input FilterAttributes array on a new Step should merge and register")]
        public void InputFilterAttributesShouldBeMergedAndRegistered()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>()
            {
                ExistingContactPluginType,
                UpdateSdkMessage,
                UpdateContactMessageFilter
            });

            Step.FilteringAttributes = new[] {"one", "two", "three"};
            Step.Register(orgService, ExistingContactPluginType.ToEntityReference(),
                UpdateSdkMessage.ToEntityReference(), UpdateContactMessageFilter.ToEntityReference());

            var registeredStep =
                (from e in context.CreateQuery<SdkMessageProcessingStep>()
                    where e.Name == Step.Name
                    select e).ToList();

            Assert.AreEqual(1, registeredStep.Count);
            Assert.AreEqual("one,two,three", registeredStep[0].FilteringAttributes);
        }

        [Test]
        [Description("Input FilterAttributes array on an existing Step should merge and update existing attribute")]
        public void InputFilterAttributesOnAnExistingStepShouldBeMergedAndUpdate()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            ExistingPluginStep.FilteringAttributes = "one,two,three,four";
            context.Initialize(new List<Entity>()
            {
                ExistingContactPluginType,
                UpdateSdkMessage,
                UpdateContactMessageFilter,
                ExistingPluginStep
            });

            Step.FilteringAttributes = new[] { "one", "two" };
            Step.Register(orgService, ExistingContactPluginType.ToEntityReference(),
                UpdateSdkMessage.ToEntityReference(), UpdateContactMessageFilter.ToEntityReference());

            var registeredStep =
                (from e in context.CreateQuery<SdkMessageProcessingStep>()
                    where e.Name == Step.Name
                    select e).ToList();

            Assert.AreEqual(1, registeredStep.Count);
            Assert.AreEqual("one,two", registeredStep[0].FilteringAttributes);
        }

        
    }
}
