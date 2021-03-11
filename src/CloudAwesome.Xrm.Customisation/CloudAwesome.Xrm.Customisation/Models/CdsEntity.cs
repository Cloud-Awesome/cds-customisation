using System.ServiceModel;
using System.Xml.Serialization;
using CloudAwesome.Xrm.Customisation.Exceptions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;

namespace CloudAwesome.Xrm.Customisation.Models
{
    public class CdsEntity
    {
        private string _solutionName;
        private string _publisherPrefix;
        private string _pluralName;
        private string _description;
        private string _primaryAttributeName;
        private int _primaryAttributeMaxLength;
        private string _primaryAttributeDescription;

        public string DisplayName { get; set; }
        public string SchemaName { get; set; }

        public string Description
        {
            get => _description;
            set => _description = string.IsNullOrEmpty(value) ? "A custom entity" : value;
        }
        public OwnershipTypes OwnershipType { get; set; }

        public string PluralName
        {
            get => _pluralName ?? this.DisplayName.Pluralise();
            set => _pluralName = string.IsNullOrEmpty(value) ? this.DisplayName.Pluralise() : value;
        }
        
        public string PrimaryAttributeName
        {
            get => _primaryAttributeName;
            set => _primaryAttributeName = value ?? "Name";
        }

        public int? PrimaryAttributeMaxLength
        {
            get => _primaryAttributeMaxLength == 0? 50 : _primaryAttributeMaxLength;
            set => _primaryAttributeMaxLength = value ?? 50;
        }

        public string PrimaryAttributeDescription
        {
            get => _primaryAttributeDescription ?? "Primary name attribute for this entity";
            set => _primaryAttributeDescription = value ?? "Primary name attribute for this entity";
        }
        public bool? IsActivity { get; set; }
        public bool? HasActivities { get; set; }
        public bool? HasNotes { get; set; }
        public bool? IsQuickCreateEnabled { get; set; }
        public bool? IsAuditEnabled { get; set; }
        public bool? IsDuplicateDetectionEnabled { get; set; }
        public bool? IsBusinessProcessEnabled { get; set; }
        public bool? IsDocumentManagementEnabled { get; set; }
        public bool? IsValidForQueue { get; set; }
        public bool? ChangeTrackingEnabled { get; set; }
        public string NavigationColour { get; set; }

        public bool AllBusinessRules { get; set; }

        [XmlArrayItem("Attribute")]
        public CdsAttribute[] Attributes { get; set; }

        [XmlArrayItem("Permissions")]
        public CdsEntityPermission[] EntityPermissions { get; set; }

        private void Create(IOrganizationService client)
        {
            var entityMetadata = new EntityMetadata()
            {
                LogicalName = this.SchemaName,
                SchemaName = this.SchemaName,
                DisplayName = this.DisplayName.CreateLabelFromString(),
                DisplayCollectionName = this.PluralName.CreateLabelFromString(),
                OwnershipType = this.OwnershipType,
                IsActivity = this.IsActivity,
                Description = this.Description.CreateLabelFromString(),
                IsQuickCreateEnabled = this.IsQuickCreateEnabled,
                IsAuditEnabled = new BooleanManagedProperty(GetBooleanValue(this.IsAuditEnabled, false)),
                IsDuplicateDetectionEnabled = new BooleanManagedProperty(GetBooleanValue(this.IsDuplicateDetectionEnabled, false)),
                IsBusinessProcessEnabled = this.IsBusinessProcessEnabled,
                IsDocumentManagementEnabled = this.IsDocumentManagementEnabled,
                IsValidForQueue = new BooleanManagedProperty(GetBooleanValue(this.IsValidForQueue, false)),
                ChangeTrackingEnabled = this.ChangeTrackingEnabled,
                HasActivities = this.HasActivities,
                HasNotes = this.HasNotes
            };

            if (this.IsActivity != null && this.IsActivity.Value)
            {
                entityMetadata.OwnershipType = OwnershipTypes.UserOwned;
            }

            var primaryAttribute = new StringAttributeMetadata()
            {
                LogicalName = string.Format($"{_publisherPrefix}_name"),
                SchemaName = string.Format($"{_publisherPrefix}_name"),
                DisplayName = string.IsNullOrEmpty(this.PrimaryAttributeName) 
                    ? "A custom entity".CreateLabelFromString() 
                    : this.PrimaryAttributeName.CreateLabelFromString(),
                MaxLength = this.PrimaryAttributeMaxLength,
                Description = this.PrimaryAttributeDescription.CreateLabelFromString()
            };

            var createEntityRequest = new CreateEntityRequest()
            {
                Entity = entityMetadata,
                SolutionUniqueName = this._solutionName,
                PrimaryAttribute = primaryAttribute
            };
            client.Execute(createEntityRequest);
        }

        private void Update(IOrganizationService client, EntityMetadata metadata)
        {
            metadata.DisplayName = string.IsNullOrEmpty(this.DisplayName) ? metadata.DisplayName: this.DisplayName.CreateLabelFromString();
            metadata.DisplayCollectionName = string.IsNullOrEmpty(this.PluralName) ? metadata.DisplayCollectionName: this.PluralName.CreateLabelFromString();
            metadata.Description = string.IsNullOrEmpty(this.Description) ? metadata.Description: this.Description.CreateLabelFromString();
            metadata.IsQuickCreateEnabled = GetBooleanValue(this.IsQuickCreateEnabled, metadata.IsQuickCreateEnabled);
            metadata.IsAuditEnabled = new BooleanManagedProperty(GetBooleanValue(this.IsAuditEnabled, metadata.IsAuditEnabled.Value));
            metadata.IsDuplicateDetectionEnabled = new BooleanManagedProperty(GetBooleanValue(this.IsDuplicateDetectionEnabled, metadata.IsDuplicateDetectionEnabled.Value));
            metadata.IsBusinessProcessEnabled = GetBooleanValue(this.IsBusinessProcessEnabled, metadata.IsBusinessProcessEnabled);
            metadata.IsDocumentManagementEnabled = GetBooleanValue(this.IsDocumentManagementEnabled, metadata.IsDocumentManagementEnabled);
            metadata.IsValidForQueue = new BooleanManagedProperty(GetBooleanValue(this.IsValidForQueue, metadata.IsValidForQueue.Value));
            metadata.ChangeTrackingEnabled = GetBooleanValue(this.ChangeTrackingEnabled, metadata.ChangeTrackingEnabled);
            metadata.HasNotes = GetBooleanValue(this.HasNotes, metadata.HasNotes);
            metadata.HasActivities = GetBooleanValue(this.HasNotes, metadata.HasActivities);

            var updateEntityRequest = new UpdateEntityRequest()
            {
                Entity = metadata,
                MergeLabels = false,
                SolutionUniqueName = this._solutionName,
            };

            client.Execute(updateEntityRequest);
        }

        public void CreateOrUpdate(IOrganizationService client, string publisherPrefix, ConfigurationManifest manifest)
        {
            this._solutionName = manifest.SolutionName;
            this._publisherPrefix = publisherPrefix;

            this.SchemaName = string.IsNullOrEmpty(this.SchemaName)
                ? this.DisplayName.GenerateLogicalNameFromDisplayName(publisherPrefix)
                : this.SchemaName;

            var existingMetadata = new EntityMetadata();
            bool existingEntity;
            try
            {
                var entity = new RetrieveEntityRequest()
                {
                    LogicalName = this.SchemaName,
                    EntityFilters = EntityFilters.Entity
                };
                existingMetadata = ((RetrieveEntityResponse) client.Execute(entity)).EntityMetadata;
                existingEntity = true;
            }
            catch (FaultException)
            {
                existingEntity = false;
            }
            
            if (existingEntity)
            {
                if (existingMetadata.IsCustomizable != null &&
                    !existingMetadata.IsCustomizable.Value)
                {
                    throw new NotCustomisableException(
                        $"Entity {this.SchemaName} is a managed entity and cannot be customised");
                }
                this.Update(client, existingMetadata);
            }
            else
            {
                this.Create(client);
            }
        }

        /// <summary>
        /// Logic for updating boolean values in CRM.
        /// </summary>
        /// <remarks>
        /// If no new value is passed through, leave as is;
        /// If new value is TRUE then allow it regardless of existing value;
        /// If the new value is false but the existing value is true then check if this transition is permitted
        /// (e.g. can't switch off activities once activated)
        /// </remarks>
        /// <param name="newValue">Value pass through from manifest</param>
        /// <param name="existingValue">Value from existing CRM metadata</param>
        /// <param name="allowTrueToFalseTransition">Does CRM allow setting a TRUE to FALSE for this field?</param>
        /// <returns></returns>
        private bool GetBooleanValue(bool? newValue, bool? existingValue, bool allowTrueToFalseTransition = true)
        {
            switch (newValue)
            {
                case null:
                    return existingValue ?? false;
                case false:
                {
                    if (existingValue == true)
                    {
                        return !allowTrueToFalseTransition;
                    }
                    break;
                }
                default:
                    return true;
            }
            return false;
        }
    }
}
