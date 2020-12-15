﻿using System.Xml.Serialization;
using CloudAwesome.Xrm.Customisation.Models;

namespace CloudAwesome.Xrm.Customisation
{
    public class PluginManifest
    {
        public string SolutionName { get; set; }

        public CdsConnection CdsConnection { get; set; }

        [XmlArrayItem("PluginAssembly")]
        public CdsPluginAssembly[] PluginAssemblies { get; set; }

        [XmlArrayItem("ServiceEndpoint")]
        public CdsServiceEndpoint[] ServiceEndpoints { get; set; }

        [XmlArrayItem("Webhook")]
        public CdsWebhook[] Webhooks { get; set; }

        [XmlArrayItem("WorkflowAssembly")]
        public CdsWorkflowAssembly[] WorkflowAssemblies { get; set; }

    }
}