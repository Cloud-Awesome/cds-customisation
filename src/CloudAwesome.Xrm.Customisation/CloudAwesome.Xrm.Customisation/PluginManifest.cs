using CloudAwesome.Xrm.Customisation.Models;

namespace CloudAwesome.Xrm.Customisation
{
    public class PluginManifest
    {
        public string SolutionName { get; set; }

        public CdsConnection CdsConnection { get; set; }

        public CdsPluginAssembly[] PluginAssemblies { get; set; }

        public CdsServiceEndpoint[] ServiceEndpoints { get; set; }

        public CdsWebhook[] Webhooks { get; set; }

        public CdsWorkflowAssembly[] WorkflowAssemblies { get; set; }

    }
}
