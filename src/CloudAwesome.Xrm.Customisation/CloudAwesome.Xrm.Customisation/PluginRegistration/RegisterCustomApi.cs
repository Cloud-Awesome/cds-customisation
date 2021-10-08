using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Customisation.Models;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation.PluginRegistration
{
    internal static class RegisterCustomApi
    {
        internal static EntityReference Run(CdsCustomApi api, EntityReference parentPluginType, string targetSolutionName,
            IOrganizationService client, TracingHelper t)
        {
            t.Debug($"Processing Step = {api.FriendlyName}");

            var createdApi = api.Register(client, parentPluginType);
            t.Info($"CustomApi {api.FriendlyName} created with ID {createdApi.Id}");

            SolutionWrapper.AddSolutionComponent(client, targetSolutionName, createdApi.Id, ComponentType.CustomApi);
            t.Debug($"Custom API '{api.FriendlyName}' added to solution {targetSolutionName}");

            return createdApi;
        }
        
    }
}