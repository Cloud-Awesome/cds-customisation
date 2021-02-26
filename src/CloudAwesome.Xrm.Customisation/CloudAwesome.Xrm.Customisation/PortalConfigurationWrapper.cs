using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Customisation.Exceptions;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation
{
    public class PortalConfigurationWrapper
    {
        public void GenerateCustomisations(PortalConfigurationManifest manifest, IOrganizationService client, TracingHelper t)
        {
            if (string.IsNullOrEmpty(manifest.Website))
            {
                throw new InvalidManifestException("Website is not specified in the manifest");
            }
            
            // TODO - sort out the required order...
            // TODO - Figure out how to have all records from all the portal models in a single list
                // So they can all be looped through together, instead of having to loop through
                // each entity one at a time...
                // Also - think about batching them...

        }
    }
}