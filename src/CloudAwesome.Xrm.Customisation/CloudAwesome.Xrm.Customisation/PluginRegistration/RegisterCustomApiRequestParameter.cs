using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Customisation.Models;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation.PluginRegistration
{
    internal static class RegisterCustomApiRequestParameter
    {
        internal static EntityReference Run(CdsRequestParameter requestParameter, EntityReference parentApi, 
            string targetSolutionName, IOrganizationService client, TracingHelper t)
        {
            t.Debug($"Processing Custom API request parameter '{requestParameter.FriendlyName}'");
            var createdParameter = requestParameter.Register(client, parentApi);
            t.Info($"Request parameter '{requestParameter.FriendlyName} created with ID {createdParameter.Id}'");

            SolutionWrapper.AddSolutionComponent(client, targetSolutionName, createdParameter.Id, ComponentType.CustomApiRequestParameter);
            t.Debug($"Request parameter '{requestParameter.FriendlyName}' added to solution {targetSolutionName}");

            return createdParameter;
        }
    }
}