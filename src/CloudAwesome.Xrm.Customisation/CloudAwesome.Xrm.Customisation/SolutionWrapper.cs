﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;

namespace CloudAwesome.Xrm.Customisation
{
    public static class SolutionWrapper
    {
        public static void AddSolutionComponent(IOrganizationService client, string solutionName,
            Guid solutionComponentId, ComponentType solutionComponentTypeCode)
        {
            if (string.IsNullOrEmpty(solutionName))
            {
                return;
            }

            client.Execute(new AddSolutionComponentRequest
            {
                ComponentType = (int)solutionComponentTypeCode,
                ComponentId = solutionComponentId,
                SolutionUniqueName = solutionName
            });
        }
    }

    // TODO - This Enum is probably woefully out of date ;)
    public enum ComponentType
    {
        All = 0,
        Entity = 1,
        Attribute = 2,
        Relationship = 3,
        AttributePicklistValue = 4,
        AttributeLookupValue = 5,
        ViewAttribute = 6,
        LocalizedLabel = 7,
        RelationshipExtraCondition = 8,
        OptionSet = 9,
        EntityRelationship = 10,
        EntityRelationshipRole = 11,
        EntityRelationshipRelationships = 12,
        ManagedProperty = 13,
        Role = 20,
        RolePrivilege = 21,
        DisplayString = 22,
        DisplayStringMap = 23,
        Form = 24,
        Organization = 25,
        SavedQuery = 26,
        Workflow = 29,
        Report = 31,
        ReportEntity = 32,
        ReportCategory = 33,
        ReportVisibility = 34,
        Attachment = 35,
        EmailTemplate = 36,
        ContractTemplate = 37,
        KbArticleTemplate = 38,
        MailMergeTemplate = 39,
        DuplicateRule = 44,
        DuplicateRuleCondition = 45,
        EntityMap = 46,
        AttributeMap = 47,
        RibbonCommand = 48,
        RibbonContextGroup = 49,
        RibbonCustomization = 50,
        RibbonRule = 52,
        RibbonTabToCommandMap = 53,
        RibbonDiff = 55,
        SavedQueryVisualization = 59,
        SystemForm = 60,
        WebResource = 61,
        SiteMap = 62,
        ConnectionRole = 63,
        HierarchyRule = 65,
        FieldSecurityProfile = 70,
        FieldPermission = 71,
        AppModule = 80,
        PluginType = 90,
        PluginAssembly = 91,
        SdkMessageProcessingStep = 92,
        SdkMessageProcessingStepImage = 93,
        ServiceEndpoint = 95,
        RoutingRule = 150,
        RoutingRuleItem = 151,
        Sla = 152,
        SlaItem = 153,
        ConvertRule = 154,
        ConvertRuleItem = 155,
        CustomApi = 10148,
        CustomApiRequestParameter = 10149,
        CustomApiResponseProperty = 10150
    }
}
