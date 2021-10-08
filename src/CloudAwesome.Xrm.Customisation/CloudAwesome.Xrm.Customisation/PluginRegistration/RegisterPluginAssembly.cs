using System.IO;
using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Customisation.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation.PluginRegistration
{
    internal static class RegisterPluginAssembly
    {
        internal static EntityReference Run(IOrganizationService client, PluginManifest manifest, 
            CdsPluginAssembly pluginAssembly, string targetSolutionName, TracingHelper t)
        {
            t.Debug($"Processing Assembly FriendlyName {pluginAssembly.FriendlyName}");

            if (!File.Exists(pluginAssembly.Assembly))
            {
                t.Critical($"Assembly {pluginAssembly.Assembly} cannot be found!");
                return null;
            }

            t.Debug($"Registering Assembly {pluginAssembly.FriendlyName}");
            var createdAssembly = pluginAssembly.Register(client);
            t.Info($"Assembly {pluginAssembly.FriendlyName} registered with ID {createdAssembly.Id}");

            SolutionWrapper.AddSolutionComponent(client, targetSolutionName, createdAssembly.Id, ComponentType.PluginAssembly);
            t.Debug($"Assembly '{pluginAssembly.FriendlyName}' added to solution {targetSolutionName}");

            return createdAssembly;
        }
        
    }
}