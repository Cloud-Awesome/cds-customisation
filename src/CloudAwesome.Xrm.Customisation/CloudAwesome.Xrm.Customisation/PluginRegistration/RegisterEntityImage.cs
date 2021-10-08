using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Customisation.Models;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation.PluginRegistration
{
    internal static class RegisterEntityImage
    {
        internal static EntityReference Run(CdsEntityImage entityImage, EntityReference parentPluginStep, 
            IOrganizationService client, TracingHelper t)
        {
            t.Debug($"Processing Entity Image = {entityImage.Name}");
            var createdImage = entityImage.Register(client, parentPluginStep);
            t.Info($"Entity image {entityImage.Name} registered with ID {createdImage.Id}");

            return createdImage;
        }
        
    }
}