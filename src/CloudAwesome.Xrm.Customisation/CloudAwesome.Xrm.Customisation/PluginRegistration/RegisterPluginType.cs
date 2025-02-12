using CloudAwesome.Xrm.Customisation.DataverseExtensions;
using CloudAwesome.Xrm.Customisation.Models;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation.PluginRegistration
{
    internal static class RegisterPluginType
    {
        internal static EntityReference Run(CdsPlugin plugin, EntityReference parentAssembly, 
            IOrganizationService client, TracingHelper t)
        {
            t.Debug($"PluginType FriendlyName = {plugin.FriendlyName}");
            var createdPluginType = plugin.Register(client, parentAssembly);
            t.Info($"Plugin {plugin.FriendlyName} registered with ID {createdPluginType.Id.ToString()}");

            return createdPluginType;
        }
        
    }
}