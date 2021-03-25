using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using CloudAwesome.Xrm.Customisation.Models;
using FakeXrmEasy;
using FluentAssertions;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.ModelTests
{
    [TestFixture]
    public class CdsPluginAssemblyTests: BaseFakeXrmTest
    {
        private CdsPluginAssembly _unitTestPluginAssembly;
        private PluginAssemblyInfo _pluginAssemblyInfo;

        [SetUp]
        public void Init()
        {
            // Mock up assembly file data and contents

            var random = new Random();
            var buffer = new byte[8000];
            random.NextBytes(buffer);

            var mockFileSystem = new MockFileSystem();
            var mockAssemblyFile = new MockFileData(buffer);
            mockFileSystem.AddFile("../UnitTestAssembly.dll", mockAssemblyFile);

            _pluginAssemblyInfo = new PluginAssemblyInfo("1.0.0.0", "neutral", "615679ec018eccbc");

            _unitTestPluginAssembly = new CdsPluginAssembly(mockFileSystem)
            {
                Name = "UnitTestAssembly",
                FriendlyName = "Updated Unit Test Assembly",
                Assembly = "../UnitTestAssembly.dll",
            };
        }

        [Test]
        [Description("Registering an assembly which already exists should update existing assembly")]
        public void Register_Existing_Assembly_Should_Update()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>()
            {
                ExistingPluginAssembly
            });
            
            _unitTestPluginAssembly.Register(orgService, _pluginAssemblyInfo);

            var updatedAssembly =
                (from a in context.CreateQuery<PluginAssembly>()
                    where a.Name == _unitTestPluginAssembly.Name
                    select a).ToList();
            
            updatedAssembly.Should().HaveCount(1);
            updatedAssembly[0].Id.Should().NotBeEmpty();
        }

        [Test]
        [Description("Registering an assembly with a more advanced version number should create a new assembly alongside the old version")]
        public void Register_Assembly_With_Higher_Version_Number_Should_Create_New()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            context.Initialize(new List<Entity>()
            {
                ExistingPluginAssembly
            });

            _pluginAssemblyInfo.Version = "2.0.0.0";
            _unitTestPluginAssembly.Register(orgService, _pluginAssemblyInfo);

            var updatedAssembly =
                (from a in context.CreateQuery<PluginAssembly>()
                    where a.Name == _unitTestPluginAssembly.Name
                    select a).ToList();

            Assert.AreEqual(2, updatedAssembly.Count);

            var updatedAssemblyV2 =
                (from a in context.CreateQuery<PluginAssembly>()
                    where a.Name == _unitTestPluginAssembly.Name && a.Version == "2.0.0.0"
                    select a).ToList();

            updatedAssemblyV2.Should().HaveCount(1);
        }

        [Test]
        [Description("Registering an assembly which doesn't exist should create a new assembly")]
        public void Register_NonExistent_Assembly_Should_Create_New()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            _unitTestPluginAssembly.Register(orgService, _pluginAssemblyInfo);

            var updatedAssembly =
                (from a in context.CreateQuery<PluginAssembly>()
                    where a.Name == _unitTestPluginAssembly.Name
                    select a).ToList();
            
            updatedAssembly.Should().HaveCount(1);
            updatedAssembly[0].Id.Should().NotBeEmpty();
        }
    }
}