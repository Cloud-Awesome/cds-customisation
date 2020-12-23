using System;
using System.ServiceModel.Description;
using System.Xml.Serialization;
using CloudAwesome.Xrm.Core;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;

namespace CloudAwesome.Xrm.Customisation.Models
{
    public class CdsServiceEndpoint
    {
        public string Name { get; set; }

        public string NamespaceAddress { get; set; }

        // aka DesignationType
        public ServiceEndpoint_Contract Contract { get; set; }

        public string Path { get; set; }

        public ServiceEndpoint_MessageFormat MessageFormat { get; set; }

        public ServiceEndpoint_AuthType AuthType { get; set; }

        public string SASKeyName { get; set; }

        public string SASKey { get; set; }

        public ServiceEndpoint_UserClaim UserClaim { get; set; }

        public string Description { get; set; }

        [XmlArrayItem("Step")]
        public CdsPluginStep[] Steps { get; set; }

        public EntityReference Register(IOrganizationService client)
        {
            var serviceEndpoint = new ServiceEndpoint()
            {
                Name = this.Name,
                NamespaceAddress = this.NamespaceAddress,
                NamespaceFormat = ServiceEndpoint_NamespaceFormat.NamespaceAddress,
                Contract = this.Contract,
                Path = this.Path,
                MessageFormat = this.MessageFormat,
                AuthType = ServiceEndpoint_AuthType.SASKey,
                SASKeyName = this.SASKeyName,
                SASKey = this.SASKey,
                Description = this.Description,
                UserClaim = ServiceEndpoint_UserClaim.None,
            };

            var existingServiceEndpointQuery = this.GetExistingServiceEndpoint();
            
            return serviceEndpoint.CreateOrUpdate(client, existingServiceEndpointQuery);
        }

        public void Unregister()
        {
            throw new NotImplementedException();
        }

        public QueryBase GetExistingServiceEndpoint()
        {
            return new QueryExpression()
            {
                EntityName = ServiceEndpoint.EntityLogicalName,
                ColumnSet = new ColumnSet(ServiceEndpoint.PrimaryIdAttribute, 
                    ServiceEndpoint.PrimaryNameAttribute),
                Criteria = new FilterExpression()
                {
                    Conditions =
                    {
                        new ConditionExpression(ServiceEndpoint.PrimaryNameAttribute, 
                            ConditionOperator.Equal, this.Name)
                    }
                }
            };
        }
    }
}
