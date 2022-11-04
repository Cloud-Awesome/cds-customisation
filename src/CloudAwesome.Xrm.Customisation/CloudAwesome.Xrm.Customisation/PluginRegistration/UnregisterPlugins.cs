using System.IO;
using System.Linq;
using CloudAwesome.Xrm.Core;
using CloudAwesome.Xrm.Customisation.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation.PluginRegistration
{
    public static class UnregisterPlugins
    {
        
        public static void Run(PluginManifest manifest, IOrganizationService client)
        {
            if (manifest.LoggingConfiguration != null)
            {
                var t = new TracingHelper(manifest.LoggingConfiguration);
                Run(manifest, client, t);
            }
            else
            {
                Run(manifest, client, t: null);
            }
        }

        public static void Run(PluginManifest manifest, IOrganizationService client, ILogger logger)
        {
            var t = new TracingHelper(logger);
            Run(manifest, client, t);
        }

        public static void Run(PluginManifest manifest, IOrganizationService client, TracingHelper t)
        {
            t.Debug($"Entering PluginWrapper.UnregisterPlugins");

            // TODO - need to clobber Custom APIs and child parameters/properties too!!

            foreach (var pluginAssembly in manifest.PluginAssemblies)
            {
                t.Debug($"Getting PluginAssemblyInfo from file {pluginAssembly.Assembly}");
                if (!File.Exists(pluginAssembly.Assembly))
                {
                    t.Critical($"Assembly {pluginAssembly.Assembly} cannot be found!");
                    continue;
                }

                var pluginAssemblyInfo = new PluginAssemblyInfo(pluginAssembly.Assembly);
                var existingAssembly = 
                    pluginAssembly.GetExistingQuery(pluginAssemblyInfo.Version)
                        .RetrieveSingleRecord(client);

                if (existingAssembly == null) return;

                var childPluginTypesResults = PluginQueries.GetChildPluginTypesQuery(existingAssembly.ToEntityReference()).RetrieveMultiple(client);
                var pluginsList = childPluginTypesResults.Entities.Select(e => e.Id).ToList();

                PluginQueries.GetChildCustomApisQuery(pluginsList).DeleteAllResults(client);

                var childStepsResults = PluginQueries.GetChildPluginStepsQuery(pluginsList).RetrieveMultiple(client);
                var pluginStepsList = childStepsResults.Entities.Select(e => e.Id).ToList();

                if (pluginStepsList.Count > 0)
                {
                    PluginQueries.GetChildEntityImagesQuery(pluginStepsList).DeleteAllResults(client);
                }

                if (pluginsList.Count > 0)
                {
                    PluginQueries.GetChildPluginStepsQuery(pluginsList).DeleteAllResults(client);
                }

                PluginQueries.GetChildPluginTypesQuery(existingAssembly.ToEntityReference()).DeleteAllResults(client);
                pluginAssembly.GetExistingQuery(pluginAssemblyInfo.Version).DeleteSingleRecord(client);

            }

            t.Debug($"Exiting PluginWrapper.UnregisterPlugins");
        }
    }
}