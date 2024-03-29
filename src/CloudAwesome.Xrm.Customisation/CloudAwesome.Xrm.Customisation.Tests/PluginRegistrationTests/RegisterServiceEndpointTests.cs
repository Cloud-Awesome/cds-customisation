﻿using System.Collections.Generic;
using System.Linq;
using CloudAwesome.Xrm.Customisation.EarlyBoundModels;
using CloudAwesome.Xrm.Customisation.PluginRegistration;
using FakeXrmEasy;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.PluginRegistrationTests
{
    
    public class RegisterServiceEndpointTests: BaseFakeXrmTest
    {
        [Test]
        [Description("Endpoint doesn't exist but is registered successfully")]
        public void ServiceEndPointDoeNotExist()
        {
            var manifestPath = $"{PluginManifestFolderPath}/plugin-manifest.xml";
            var manifest = SerialisationWrapper.DeserialiseFromFile<PluginManifest>(manifestPath);

            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();
            context.Initialize(new List<Entity>()
            {
                UpdateSdkMessage,
                UpdateContactMessageFilter,
                UpdateAccountMessageFilter
            });

            var pluginWrapper = new PluginWrapper();
            pluginWrapper.RegisterServiceEndpoints(manifest, orgService);

            var postRegisteredEndpoints =
                (from e in context.CreateQuery<ServiceEndpoint>()
                    where e.Name == "TesterQueue"
                    select e).ToList();

            Assert.AreEqual(1, postRegisteredEndpoints.Count);
            Assert.IsNotNull(postRegisteredEndpoints[0].Id);

        }

        [Test]
        [Description("Endpoint already exists and Clobber is false but endpoint and steps are not duplicated")]
        public void ServiceEndpointAlreadyExistsWithNoClobber()
        {
            var manifestPath = $"{PluginManifestFolderPath}/plugin-manifest.xml";
            var manifest = SerialisationWrapper.DeserialiseFromFile<PluginManifest>(manifestPath);

            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();
            context.Initialize(new List<Entity>()
            {
                ExistingTesterQueueEndpoint,
                UpdateSdkMessage,
                UpdateContactMessageFilter,
                UpdateAccountMessageFilter
            });

            manifest.Clobber = false;

            var pluginWrapper = new PluginWrapper();
            pluginWrapper.RegisterServiceEndpoints(manifest, orgService);

            var postRegisteredEndpoints =
                (from e in context.CreateQuery<ServiceEndpoint>()
                    where e.Name == "TesterQueue"
                    select e).ToList();

            var postRegisteredEndpointSteps =
                (from s in context.CreateQuery<SdkMessageProcessingStep>()
                    where Equals(s.EventHandler, postRegisteredEndpoints.FirstOrDefault().ToEntityReference())
                    select s).ToList();

            Assert.AreEqual(1, postRegisteredEndpoints.Count);
            Assert.AreEqual(2, postRegisteredEndpointSteps.Count);
        }

        [Test]
        [Description("Endpoint already exists and Clobber is true but endpoint is not duplicated")]
        public void ServiceEndpointAlreadyExistsWithClobber()
        {
            var manifestPath = $"{PluginManifestFolderPath}/plugin-manifest.xml";
            var manifest = SerialisationWrapper.DeserialiseFromFile<PluginManifest>(manifestPath);

            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();
            context.Initialize(new List<Entity>()
            {
                ExistingTesterQueueEndpoint,
                UpdateSdkMessage,
                UpdateContactMessageFilter,
                UpdateAccountMessageFilter
            });

            var pluginWrapper = new PluginWrapper();
            pluginWrapper.RegisterServiceEndpoints(manifest, orgService);

            var postRegisteredEndpoints =
                (from e in context.CreateQuery<ServiceEndpoint>()
                    where e.Name == "TesterQueue"
                    select e).ToList();

            Assert.AreEqual(1, postRegisteredEndpoints.Count);
        }
    }
}
