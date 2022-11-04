using System.ServiceModel;
using CloudAwesome.Xrm.Core;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace CloudAwesome.Xrm.Customisation.ConfigurationManagement
{
    public static class GlobalOptionSets
    {
        /// <summary>
        /// Create or update global option sets from ConfigurationManifest
        /// </summary>
        /// <param name="manifest"></param>
        /// <param name="client"></param>
        /// <param name="t"></param>
        /// <param name="publisherPrefix"></param>
        public static void Generate(ConfigurationManifest manifest, IOrganizationService client, TracingHelper t, string publisherPrefix)
        {
            if (manifest.OptionSets == null)
            {
                t.Debug($"No global optionsets in manifest to be processed");
            }
            else
            {
                foreach (var optionSet in manifest.OptionSets)
                {
                    t.Debug($"Processing global option set: {optionSet.DisplayName}");

                    var targetOptionSet = new OptionSetMetadata
                    {
                        Name = string.IsNullOrEmpty(optionSet.SchemaName) ? optionSet.DisplayName.GenerateLogicalNameFromDisplayName(publisherPrefix) : optionSet.SchemaName,
                        DisplayName = new Label(optionSet.DisplayName, 1033),
                        IsGlobal = true,
                        OptionSetType = OptionSetType.Picklist,
                    };
                    foreach (var option in optionSet.Items)
                    {
                        targetOptionSet.Options.Add(new OptionMetadata(option.CreateLabelFromString(), null));
                    }

                    bool existingOptionSet;
                    try
                    {
                        var retrieveOptionSet = (RetrieveOptionSetResponse) client.Execute(new RetrieveOptionSetRequest()
                        {
                            Name = optionSet.SchemaName
                        });
                        var retrievedMetadataAttribute = retrieveOptionSet.OptionSetMetadata;
                        targetOptionSet.DisplayName = optionSet.DisplayName == null
                            ? retrievedMetadataAttribute.DisplayName
                            : optionSet.DisplayName.CreateLabelFromString();
                        targetOptionSet.MetadataId = retrievedMetadataAttribute.MetadataId;

                        existingOptionSet = true;
                    }
                    catch (FaultException)
                    {
                        existingOptionSet = false;
                    }

                    if (existingOptionSet)
                    {
                        client.Execute(new UpdateOptionSetRequest()
                        {
                            MergeLabels = true,
                            OptionSet = targetOptionSet
                        });
                        if (targetOptionSet.MetadataId != null)
                        {
                            SolutionWrapper.AddSolutionComponent(client, manifest.SolutionName, targetOptionSet.MetadataId.Value, ComponentType.OptionSet);
                        }
                    }
                    else
                    {
                        var response = (CreateOptionSetResponse) client.Execute(new CreateOptionSetRequest()
                        {
                            OptionSet = targetOptionSet
                        });
                        SolutionWrapper.AddSolutionComponent(client, manifest.SolutionName, response.OptionSetId, ComponentType.OptionSet);
                    }
                    
                    t.Info($"Successfully processed global option set: {optionSet.DisplayName}");
                }
            }
        }
    }
}