using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Customisation.Models;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation.PluginRegistration
{
    internal static class RegisterCustomerApiResponseProperty
    {
        internal static EntityReference Run(CdsResponseProperty responseProperty, EntityReference parentApi,
            string targetSolutionName, IOrganizationService client, TracingHelper t)
        {
            t.Debug($"Processing Custom API response property '{responseProperty.FriendlyName}'");
            var createdProperty = responseProperty.Register(client, parentApi);
            t.Info($"Response property '{responseProperty.FriendlyName} created with ID {createdProperty.Id}'");

            SolutionWrapper.AddSolutionComponent(client, targetSolutionName, createdProperty.Id, ComponentType.CustomApiResponseProperty);
            t.Debug($"Response property '{responseProperty.FriendlyName}' added to solution {targetSolutionName}");

            return createdProperty;
        }
        
    }
}