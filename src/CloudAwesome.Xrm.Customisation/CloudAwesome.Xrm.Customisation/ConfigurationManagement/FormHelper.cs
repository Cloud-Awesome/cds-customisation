using System;
using System.Linq;
using System.Xml;
using CloudAwesome.Xrm.Customisation.DataverseExtensions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Customisation.ConfigurationManagement
{
    public static class FormHelper
    {
        public enum FormType { Undefined = -1, Dashboard = 0, AppointmentBook = 1, Main = 2, MiniCampaignBo = 3, Preview = 4, Mobile = 5, QuickView = 6, QuickCreate = 7, Other = 100, MainBackup = 101, AppointmentBookBackup = 102 }
        
        public static void AddAttributeToForm(AttributeMetadata attributeMetadata, ref XmlDocument formXml)
        {
            if (formXml == null)
            {
                formXml = new XmlDocument();
                formXml.LoadXml(_formXmlTemplate);
            }
            
            var rowNode = formXml.SelectSingleNode("//rows");

            var displayName = attributeMetadata.DisplayName?.UserLocalizedLabel?.Label;
            if (string.IsNullOrEmpty(displayName))
                if (attributeMetadata.DisplayName != null)
                    displayName = attributeMetadata.DisplayName.LocalizedLabels?.FirstOrDefault()?.Label;
            
            Guid controlClassId = FormHelper.GetFormControlClassId(attributeMetadata);
            if (controlClassId == Guid.Empty) throw new Exception($"Cannot determine Control Class ID for '{displayName}'.");
            
            rowNode.InnerXml += $"<row>" +
                                    $"<cell id=\"{{00000000-0000-0000-0000-000000000000}}\" showlabel=\"true\" locklevel=\"0\">" +
                                    $"<labels>" +
                                        $"<label description=\"{displayName}\" languagecode=\"1033\" /></labels>" +
                                        $"<control id=\"{attributeMetadata.LogicalName}\" classid=\"{controlClassId.ToString("B")}\" datafieldname=\"{attributeMetadata.LogicalName}\" disabled=\"false\" />" +
                                    $"</cell>" +
                                 $"</row>";
        }
        
        public static void UpdateFormXml(IOrganizationService client, string entitySchemaName, XmlDocument formXml, 
            string formName = null, FormType formType = FormType.Main)
        {
            QueryExpression formQuery = GetExistingFormQuery(entitySchemaName, formName, formType);
            var formEntity = formQuery.RetrieveSingleRecord(client);

            formEntity["formxml"] = formXml.InnerXml;
            formEntity.Update(client);
        }
        
        public static void UpdateFormXml(IOrganizationService orgService, Guid formId, string formXml)
        {
            Entity formEntity = new Entity("systemform") { Id = formId };
            formEntity["formxml"] = formXml;
            orgService.Update(formEntity);
        }

        public static string ReplaceFormIds(XmlDocument formXml)
        {
            ReplaceFormId(formXml, "//header/@id");
            ReplaceFormId(formXml, "//tab/@id");
            ReplaceFormId(formXml, "//tab/@labelid");
            ReplaceFormId(formXml, "//section/@id");
            ReplaceFormId(formXml, "//section/@labelid");
            ReplaceFormId(formXml, "//cell/@id");
            ReplaceFormId(formXml, "//cell/@labelid");
            ReplaceFormId(formXml, "//control/@uniqueid");
            ReplaceFormId(formXml, "//Handler/@handlerUniqueId");
            return formXml.OuterXml;
        }
        
        private static void ReplaceFormId(XmlDocument xmlDoc, string xPath)
        {
            foreach (XmlAttribute el in xmlDoc.SelectNodes(xPath)) { el.Value = Guid.NewGuid().ToString("B"); }
        }
        
        public static Guid GetFormControlClassId(AttributeMetadata attrMeta)
        {
            // See https://msdn.microsoft.com/en-us/library/gg334472.aspx#Anchor_2

            if (attrMeta.AttributeType != null)
                switch (attrMeta.AttributeType.Value)
                {
                    case AttributeTypeCode.BigInt:
                    case AttributeTypeCode.Integer:
                        return new Guid("C6D124CA-7EDA-4A60-AEA9-7FB8D318B68F");

                    case AttributeTypeCode.Boolean:
                        return new Guid("B0C6723A-8503-4FD7-BB28-C8A06AC933C2");

                    case AttributeTypeCode.Lookup:

                        return new Guid("270BD3DB-D9AF-4782-9025-509E298DEC0A");

                    case AttributeTypeCode.DateTime:
                        return new Guid("5B773807-9FB2-42DB-97C3-7A91EFF8ADFF");

                    case AttributeTypeCode.Decimal:
                    case AttributeTypeCode.Double:
                        return new Guid("C3EFE0C3-0EC6-42BE-8349-CBD9079DFD8E");

                    case AttributeTypeCode.Memo:
                        return new Guid("E0DECE4B-6FC8-4A8F-A065-082708572369");

                    case AttributeTypeCode.Money:
                        return new Guid("533B9E00-756B-4312-95A0-DC888637AC78");

                    case AttributeTypeCode.PartyList:
                        return new Guid("CBFB742C-14E7-4A17-96BB-1A13F7F64AA2");

                    case AttributeTypeCode.Picklist:
                        return new Guid("3EF39988-22BB-4f0b-BBBE-64B5A3748AEE");

                    case AttributeTypeCode.String:
                        StringAttributeMetadata stringAttrMeta = (StringAttributeMetadata) attrMeta;
                        switch (stringAttrMeta.Format.Value)
                        {
                            case StringFormat.Email:
                                return new Guid("ADA2203E-B4CD-49BE-9DDF-234642B43B52");

                            case StringFormat.Phone:
                                return new Guid("8C10015A-B339-4982-9474-A95FE05631A5");

                            case StringFormat.Text:
                                return new Guid("4273EDBD-AC1D-40D3-9FB2-095C621B552D");

                            case StringFormat.TextArea:
                                return new Guid("E0DECE4B-6FC8-4A8F-A065-082708572369");

                            case StringFormat.TickerSymbol:
                                return new Guid("1E1FC551-F7A8-43AF-AC34-A8DC35C7B6D4");

                            case StringFormat.Url:
                                return new Guid("71716B6C-711E-476C-8AB8-5D11542BFB47");

                            default:
                                throw new NotSupportedException("String Format not supported");
                        }

                    case AttributeTypeCode.Virtual:
                        switch (attrMeta.AttributeTypeName.Value)
                        {
                            case "MultiSelectPicklistType":
                                return new Guid("4AA28AB7-9C13-4F57-A73D-AD894D048B5F");

                            default:
                                return Guid.Empty;
                        }
                        
                    // TODO implement these other attribute types
                    case AttributeTypeCode.Customer:
                    case AttributeTypeCode.Owner:
                    case AttributeTypeCode.State:
                    case AttributeTypeCode.Status:
                    case AttributeTypeCode.Uniqueidentifier:
                    case AttributeTypeCode.CalendarRules:
                    case AttributeTypeCode.ManagedProperty:
                    case AttributeTypeCode.EntityName:
                    default:
                        return Guid.Empty;
                }
            return  Guid.Empty;
        }
        
        public static QueryExpression GetExistingFormQuery(string entityType, string formName = null, FormType formType = FormType.Main)
        {
            // Form Types: https://docs.microsoft.com/en-us/dynamics365/customer-engagement/developer/entities/systemform#BKMK_Type

            QueryExpression query = new QueryExpression("systemform");
            query.Criteria.AddCondition("objecttypecode", ConditionOperator.Equal, entityType);
            if (formType >= 0) query.Criteria.AddCondition("type", ConditionOperator.Equal, (int)formType);
            if (!string.IsNullOrEmpty(formName)) query.Criteria.AddCondition("name", ConditionOperator.Equal, formName);

            return query;
        }
        
        private static string _formXmlTemplate = 
            @"<form>
                <tabs>
                  <tab name=""tab0"" verticallayout=""true"" id=""{00000000-0000-0000-0000-000000000000}"" IsUserDefined=""0"">
                    <labels>
                      <label description=""General"" languagecode=""1033"" />
                    </labels>
                    <columns>
                      <column width=""100%"">
                        <sections>
                          <section name=""default"" showlabel=""false"" showbar=""false"" columns=""1"" id=""{00000000-0000-0000-0000-000000000000}"" IsUserDefined=""0"">
                            <labels>
                              <label description=""Default"" languagecode=""1033"" />
                            </labels>
                            <rows>
                            </rows>
                          </section>
                        </sections>
                      </column>
                    </columns>
                  </tab>
                </tabs>
              </form>";

    }
}