using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CloudAwesome.Xrm.Core.Models;
using CloudAwesome.Xrm.Customisation.Exceptions;
using CloudAwesome.Xrm.Customisation.PluginRegistration;
using CloudAwesome.Xrm.Customisation.Tests.Stubs;
using FakeXrmEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.PluginRegistrationTests
{
    public class RegisterPluginsTests: BaseFakeXrmTest
    {
        [Test]
        [Description("Plugin assembly and steps don't already exist and are registered successfully")]
        public void New_Plugin_Assembly_And_Steps_Should_Be_Registered()
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
            postRegisteredAssemblies.FirstOrDefault()?.Name.Should().Be("SamplePluginAssembly");
        }
        
        [Test]
        [Description("Plugin assembly exists with no steps and are registered/updated successfully")]
        public void Existing_Assembly_With_No_Plugins_Should_Update_Assembly_With_New_Plugins()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();
            context.Initialize(new List<Entity>()
            {
                UpdateSdkMessage,
                CreateSdkMessage,
                UpdateContactMessageFilter,
                UpdateAccountMessageFilter,
                CreateContactMessageFilter,
                SamplePluginAssembly
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

        [Test]
        [Description("Plugin registration should accept a custom ILogger input")]
        public void Plugin_registration_helper_should_accept_custom_ILogger_parameter()
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

            var logger = new StubLogger();
            var pluginWrapper = new PluginWrapper();
            pluginWrapper.RegisterPlugins(SampleFullPluginManifest, orgService, logger);
            
            var postRegisteredAssemblies = 
                (from a in context.CreateQuery<PluginAssembly>()
                    where a.Name == "SamplePluginAssembly" 
                    select a).ToList();

            // Stub logger only stores the last call
            logger.ResponseMessage.Should().Be("Exiting PluginWrapper.RegisterPlugins");
            logger.ResponseLogLevel.Should().Be(LogLevel.Debug);
            
            postRegisteredAssemblies.Should().HaveCount(1);
        }

        [Test]
        [Description("Plugin wrapper consumes tracing configuration from manifest")]
        public void Plugin_wrapper_consumes_tracing_configuration_from_manifest()
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

            SampleFullPluginManifest.LoggingConfiguration = new LoggingConfiguration()
            {
                LoggerConfigurationType = LoggerConfigurationType.Console,
                LogLevelToTrace = LogLevel.Debug
            };

            var originalConsole = Console.Out;
            var testConsole = new StringWriter();
            Console.SetOut(testConsole);
            
            var pluginWrapper = new PluginWrapper();
            pluginWrapper.RegisterPlugins(SampleFullPluginManifest, orgService);
            
            Console.SetOut(originalConsole);
            testConsole.ToString().Should().Contain("Exiting PluginWrapper.RegisterPlugins");
        }

        [Test]
        [Description("Invalid manifest should throw an InvalidManifestException")]
        public void Invalid_plugin_manifest_should_throw_correct_exception()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var pluginWrapper = new PluginWrapper();
            Action sut = () => pluginWrapper.RegisterPlugins(InvalidPluginManifest, orgService);

            sut.Should().Throw<InvalidManifestException>();
        }
        
        [Test]
        public void Register_Plugins_With_Invalid_Manifest_Throws_InvalidManifestException()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var stubLogger = new StubLogger();
            var manifest = new PluginManifest();
            var pluginWrapper = new PluginWrapper();
            
            Action sut = () => pluginWrapper.RegisterPlugins(manifest, orgService, stubLogger);

            sut.Should()
                .Throw<InvalidManifestException>()
                .WithMessage("*'CDS Connection' must not be empty*");
            
            stubLogger.ResponseMessage.Should().Contain("Manifest is invalid");
            stubLogger.ResponseLogLevel.Should().Be(LogLevel.Critical);
        }
        
        [Test]
        [Description("Invalid manifest should throw an InvalidManifestException")]
        public void When_Assembly_Is_Not_Found_Should_Throw_Correct_Exception()
        {
            SampleFullPluginManifest.PluginAssemblies[0].Assembly =
                "this_file_doesnt_exist";
            
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var stubLogger = new StubLogger();
            var pluginWrapper = new PluginWrapper();
            
            pluginWrapper.RegisterPlugins(SampleFullPluginManifest, orgService, stubLogger);
            stubLogger.AllLogs.Should()
                .Contain($"Critical: Assembly {SampleFullPluginManifest.PluginAssemblies[0].Assembly} cannot be found!");

        }
        
        [Test]
        [Description("Plugin registration should exit after assembly registration if manifest is set to update only")]
        public void Plugin_registration_should_exit_after_assembly_reg_if_update_only()
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
            SampleFullPluginManifest.UpdateAssemblyOnly = true;
            
            var pluginWrapper = new PluginWrapper();
            pluginWrapper.RegisterPlugins(SampleFullPluginManifest, orgService);
            
            var postRegisteredAssemblies = 
                (from a in context.CreateQuery<PluginAssembly>()
                    where a.Name == "SamplePluginAssembly" 
                    select a).ToList();

            var postRegistrationPlugins =
                (from p in context.CreateQuery<PluginType>()
                    select p).ToList();

            postRegisteredAssemblies.Should().HaveCount(1, "assembly should always be registered");
            postRegistrationPlugins.Should().HaveCount(0, "would be 2 if UpdateAssemblyOnly == false"); 
        }

        [Test]
        public void Plugin_registration_should_register_new_custom_api_steps()
        {
            var context = new XrmFakedContext();
            var orgService = context.GetOrganizationService();

            var pluginWrapper = new PluginWrapper();
            pluginWrapper.RegisterPlugins(SampleCustomApiManifest, orgService);

            var registeredApis =
                (from a in context.CreateQuery<CustomAPI>()
                    select a).ToList();

            var registeredInputParams =
                (from p in context.CreateQuery<CustomAPIRequestParameter>()
                    select p).ToList();
            
            registeredApis.Should().HaveCount(1);
            registeredInputParams.Should().HaveCount(2);
        }
    }
}
