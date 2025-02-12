using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
//using CloudAwesome.MarkdownMaker;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;

namespace CloudAwesome.Xrm.Customisation.DocumentationGenerator
{
    [ExcludeFromCodeCoverage] // Alpha dev. Parked for now
    public class DocumentationGenerator
    {
        // c.f. https://dev.azure.com/cloud-awesome/_git/Argus?path=%2FAdministration%2FAdmin%20Library%2FDocumentationHelper.cs&version=GBdevelop&line=16&lineEnd=17&lineStartColumn=1&lineEndColumn=1&lineStyle=plain&_a=contents

        private IEnumerable<string> EntityNamesInSolution;
        private IEnumerable<string> ExcludeEntitiesFromErd = new[] { "processsession", "syncerror", "phonetocaseprocess", "queueitem" };

        public void GenerateDocumentForSolution(IOrganizationService client, string solutionName, string outputFolder)
        {
            var entitiesInSolution = SolutionWrapper.RetrieveSolutionComponents(client, solutionName, ComponentType.Entity);
            var entityIds = entitiesInSolution.Entities.Select(
                e => Guid.Parse(e.Attributes["objectid"].ToString()));

            var allMetadata = MetadataWrapper.GetAllEntityMetadata(client);
            var solutionMetadata = allMetadata.Where(
                e => e.MetadataId != null && entityIds.Contains(e.MetadataId.Value));

            EntityNamesInSolution = solutionMetadata.Select(e => e.LogicalName);
            
            // foreach (var entityMetadata in solutionMetadata)
            // {
            //     GenerateEntityDocumentation(client, entityMetadata, $"{outputFolder}");
            // }
            
            GenerateEntityRelationshipDiagram(solutionMetadata, outputFolder);
        }
        
        public void GenerateEntityDocumentation(IOrganizationService client, EntityMetadata entityMetadata, string folderPath)
        {
            // Top-level entity
            /*var document = new MdDocument($"{folderPath}/{entityMetadata.LogicalName}.md")
                .Add(new MdHeader(entityMetadata.DisplayName.UserLocalizedLabel?.Label, 1))
                .Add(new MdParagraph(entityMetadata.Description.UserLocalizedLabel?.Label))
                .Add(new MdTable()
                    .AddColumn(new MdPlainText("Setting"))
                    .AddColumn(new MdPlainText("Value"))
                    .AddRow(new MdTableRow()
                        .AddCell(new MdPlainText("Collection Name"))
                        .AddCell(new MdPlainText(entityMetadata.DisplayCollectionName.UserLocalizedLabel?.Label)))
                    .AddRow(new MdTableRow()
                        .AddCell(new MdPlainText("Logical Name"))
                        .AddCell(new MdPlainText(entityMetadata.LogicalName)))
                    .AddRow(new MdTableRow()
                        .AddCell(new MdPlainText("Primary attribute name"))
                        .AddCell(new MdPlainText(entityMetadata.PrimaryIdAttribute)))
                    .AddRow(new MdTableRow()
                        .AddCell(new MdPlainText("Object Type Code"))
                        .AddCell(new MdPlainText(entityMetadata.ObjectTypeCode.ToString())))
                    .AddRow(new MdTableRow()
                        .AddCell(new MdPlainText("Is custom table"))
                        .AddCell(new MdPlainText(entityMetadata.IsCustomEntity != null &&
                                                 entityMetadata.IsCustomEntity.Value
                            ? "Yes"
                            : "No")))
                )
                .Add(new MdHorizontalLine());
            
            // Attributes
            document.Add(new MdHeader("Attributes", 2));
            
            var attributesTable =
                new MdTable()
                    .AddColumn(new MdPlainText("Display Name"))
                    .AddColumn(new MdPlainText("Logical Name"))
                    .AddColumn(new MdPlainText("Datatype"))
                    .AddColumn(new MdPlainText("Description"));

            foreach (var attribute in entityMetadata.Attributes)
            {
                if (!string.IsNullOrEmpty(attribute.DisplayName.UserLocalizedLabel?.Label))
                {
                    attributesTable.AddRow(new MdTableRow()
                        .AddCell(new MdPlainText(attribute.DisplayName.UserLocalizedLabel?.Label))
                        .AddCell(new MdPlainText(attribute.LogicalName))
                        .AddCell(new MdPlainText(attribute.AttributeType.GetEnumMemberValue<EnumMemberAttribute>().Value))
                        .AddCell(new MdPlainText(attribute.Description.UserLocalizedLabel?.Label))
                    );   
                }
            }

            document
                .Add(attributesTable)
                .Add(new MdParagraph(""));
                
            // Relationships
            document.Add(new MdHeader("Relationships", 2));

            if (entityMetadata.OneToManyRelationships.Length > 0)
            {
                document.Add(new MdHeader("One to many relationships", 3));

                var oneToManyTable = new MdTable()
                    .AddColumn(new MdPlainText("Related entity"))
                    .AddColumn(new MdPlainText("Foreign key"))
                    .AddColumn(new MdPlainText("Schema name"));
                
                 foreach (var relationship in entityMetadata.OneToManyRelationships)
                 {
                     oneToManyTable.AddRow(new MdTableRow()
                         .AddCell(new MdPlainText(relationship.ReferencingEntity))
                         .AddCell(new MdPlainText(relationship.ReferencingAttribute))
                         .AddCell(new MdPlainText(relationship.SchemaName)));
                 }

                 document
                     .Add(oneToManyTable)
                     .Add(new MdParagraph(""));;
            }

            if (entityMetadata.ManyToOneRelationships.Length > 0)
            {
                document.Add(new MdHeader("Many to relationships", 3));

                var oneToManyTable = new MdTable()
                    .AddColumn(new MdPlainText("Primary entity"))
                    .AddColumn(new MdPlainText("Foreign key"))
                    .AddColumn(new MdPlainText("Schema name"));
                
                foreach (var relationship in entityMetadata.OneToManyRelationships)
                {
                    oneToManyTable.AddRow(new MdTableRow()
                        .AddCell(new MdPlainText(relationship.ReferencingEntity))
                        .AddCell(new MdPlainText(relationship.ReferencingAttribute))
                        .AddCell(new MdPlainText(relationship.SchemaName)));
                }

                document
                    .Add(oneToManyTable)
                    .Add(new MdParagraph(""));;
            }

            document.Save();*/

        }
        
        public void GenerateEntityRelationshipDiagram(IEnumerable<EntityMetadata> solutionMetadata, string folderPath)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"erDiagram \n");

            foreach (var entity in solutionMetadata)
            {
                if (ExcludeEntitiesFromErd.Contains(entity.LogicalName))
                {
                    continue;
                }

                stringBuilder.Append($"\t{entity.LogicalName}\n");
                
                foreach (var relationship in entity.OneToManyRelationships)
                {
                    if (EntityNamesInSolution.Contains(relationship.ReferencingEntity) && 
                        !ExcludeEntitiesFromErd.Contains(relationship.ReferencingEntity))
                    {
                        stringBuilder.Append($"\t{entity.LogicalName}" +
                                             " ||--o{ " +
                                             $"{relationship.ReferencingEntity} : \" \"\n");   
                    }
                }
            }

            var docFxHeader = $"---{Environment.NewLine}" +
                              $"documentType: Erd{Environment.NewLine}" +
                              $"---{Environment.NewLine}";

            /*var document = new MdDocument($"{folderPath}/erd.md")
                .Add(new MdPlainText(docFxHeader))
                .Add(new MdHeader("Entity relationship diagram", 1))
                .Add(new MdCodeBlock(stringBuilder.ToString(), "mermaid"))
                .Save();*/

        }

        public void GenerateSecurityModelDocumentation()
        {
            
        }

        public void GenerateAppDocumentation()
        {
            
        }

        public void GenerateFlowDocumentation()
        {
            
        }

        public void GenerateActionsDocumentation()
        {
            
        } 
    }
}