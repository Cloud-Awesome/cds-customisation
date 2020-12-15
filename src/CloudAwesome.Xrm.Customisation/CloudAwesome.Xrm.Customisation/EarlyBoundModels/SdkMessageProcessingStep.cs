//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CloudAwesome.Xrm.Customisation
{
	
	[System.Runtime.Serialization.DataContractAttribute()]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.45")]
	public enum SdkMessageProcessingStepState
	{
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Enabled = 0,
		
		[System.Runtime.Serialization.EnumMemberAttribute()]
		Disabled = 1,
	}
	
	/// <summary>
	/// Stage in the execution pipeline that a plug-in is to execute.
	/// </summary>
	[System.Runtime.Serialization.DataContractAttribute()]
	[Microsoft.Xrm.Sdk.Client.EntityLogicalNameAttribute("sdkmessageprocessingstep")]
	[System.CodeDom.Compiler.GeneratedCodeAttribute("CrmSvcUtil", "9.1.0.45")]
	public partial class SdkMessageProcessingStep : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, System.ComponentModel.INotifyPropertyChanged
	{
		
		public static class Fields
		{
			public const string AsyncAutoDelete = "asyncautodelete";
			public const string CanUseReadOnlyConnection = "canusereadonlyconnection";
			public const string ComponentState = "componentstate";
			public const string Configuration = "configuration";
			public const string CreatedBy = "createdby";
			public const string CreatedOn = "createdon";
			public const string CreatedOnBehalfBy = "createdonbehalfby";
			public const string CustomizationLevel = "customizationlevel";
			public const string Description = "description";
			public const string EventExpander = "eventexpander";
			public const string EventHandler = "eventhandler";
			public const string FilteringAttributes = "filteringattributes";
			public const string ImpersonatingUserId = "impersonatinguserid";
			public const string IntroducedVersion = "introducedversion";
			public const string InvocationSource = "invocationsource";
			public const string IsCustomizable = "iscustomizable";
			public const string IsHidden = "ishidden";
			public const string IsManaged = "ismanaged";
			public const string Mode = "mode";
			public const string ModifiedBy = "modifiedby";
			public const string ModifiedOn = "modifiedon";
			public const string ModifiedOnBehalfBy = "modifiedonbehalfby";
			public const string Name = "name";
			public const string OrganizationId = "organizationid";
			public const string OverwriteTime = "overwritetime";
			public const string PluginTypeId = "plugintypeid";
			public const string Rank = "rank";
			public const string SdkMessageFilterId = "sdkmessagefilterid";
			public const string SdkMessageId = "sdkmessageid";
			public const string SdkMessageProcessingStepId = "sdkmessageprocessingstepid";
			public const string Id = "sdkmessageprocessingstepid";
			public const string SdkMessageProcessingStepIdUnique = "sdkmessageprocessingstepidunique";
			public const string SdkMessageProcessingStepSecureConfigId = "sdkmessageprocessingstepsecureconfigid";
			public const string SolutionId = "solutionid";
			public const string Stage = "stage";
			public const string StateCode = "statecode";
			public const string StatusCode = "statuscode";
			public const string SupportedDeployment = "supporteddeployment";
			public const string VersionNumber = "versionnumber";
			public const string plugintype_sdkmessageprocessingstep = "plugintype_sdkmessageprocessingstep";
			public const string plugintypeid_sdkmessageprocessingstep = "plugintypeid_sdkmessageprocessingstep";
			public const string sdkmessagefilterid_sdkmessageprocessingstep = "sdkmessagefilterid_sdkmessageprocessingstep";
			public const string sdkmessageid_sdkmessageprocessingstep = "sdkmessageid_sdkmessageprocessingstep";
			public const string sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep = "sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep";
			public const string serviceendpoint_sdkmessageprocessingstep = "serviceendpoint_sdkmessageprocessingstep";
		}
		
		/// <summary>
		/// Default Constructor.
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode()]
		public SdkMessageProcessingStep() : 
				base(EntityLogicalName)
		{
		}
		
		public const string EntityLogicalName = "sdkmessageprocessingstep";
		
		public const string EntitySchemaName = "SdkMessageProcessingStep";
		
		public const string PrimaryIdAttribute = "sdkmessageprocessingstepid";
		
		public const string PrimaryNameAttribute = "name";
		
		public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
		
		public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;
		
		[System.Diagnostics.DebuggerNonUserCode()]
		private void OnPropertyChanged(string propertyName)
		{
			if ((this.PropertyChanged != null))
			{
				this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
			}
		}
		
		[System.Diagnostics.DebuggerNonUserCode()]
		private void OnPropertyChanging(string propertyName)
		{
			if ((this.PropertyChanging != null))
			{
				this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
			}
		}
		
		/// <summary>
		/// Indicates whether the asynchronous system job is automatically deleted on completion.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("asyncautodelete")]
		public System.Nullable<bool> AsyncAutoDelete
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("asyncautodelete");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("AsyncAutoDelete");
				this.SetAttributeValue("asyncautodelete", value);
				this.OnPropertyChanged("AsyncAutoDelete");
			}
		}
		
		/// <summary>
		/// Identifies whether a SDK Message Processing Step type will be ReadOnly or Read Write. false - ReadWrite, true - ReadOnly 
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("canusereadonlyconnection")]
		public System.Nullable<bool> CanUseReadOnlyConnection
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("canusereadonlyconnection");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("CanUseReadOnlyConnection");
				this.SetAttributeValue("canusereadonlyconnection", value);
				this.OnPropertyChanged("CanUseReadOnlyConnection");
			}
		}
		
		/// <summary>
		/// For internal use only.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("componentstate")]
		public virtual ComponentState? ComponentState
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return ((ComponentState?)(EntityOptionSetEnum.GetEnum(this, "componentstate")));
			}
		}
		
		/// <summary>
		/// Step-specific configuration for the plug-in type. Passed to the plug-in constructor at run time.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("configuration")]
		public string Configuration
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("configuration");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Configuration");
				this.SetAttributeValue("configuration", value);
				this.OnPropertyChanged("Configuration");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who created the SDK message processing step.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdby")]
		public Microsoft.Xrm.Sdk.EntityReference CreatedBy
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdby");
			}
		}
		
		/// <summary>
		/// Date and time when the SDK message processing step was created.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdon")]
		public System.Nullable<System.DateTime> CreatedOn
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("createdon");
			}
		}
		
		/// <summary>
		/// Unique identifier of the delegate user who created the sdkmessageprocessingstep.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("createdonbehalfby")]
		public Microsoft.Xrm.Sdk.EntityReference CreatedOnBehalfBy
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("createdonbehalfby");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("CreatedOnBehalfBy");
				this.SetAttributeValue("createdonbehalfby", value);
				this.OnPropertyChanged("CreatedOnBehalfBy");
			}
		}
		
		/// <summary>
		/// Customization level of the SDK message processing step.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("customizationlevel")]
		public System.Nullable<int> CustomizationLevel
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("customizationlevel");
			}
		}
		
		/// <summary>
		/// Description of the SDK message processing step.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("description")]
		public string Description
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("description");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Description");
				this.SetAttributeValue("description", value);
				this.OnPropertyChanged("Description");
			}
		}
		
		/// <summary>
		/// Configuration for sending pipeline events to the Event Expander service.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("eventexpander")]
		public string EventExpander
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("eventexpander");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("EventExpander");
				this.SetAttributeValue("eventexpander", value);
				this.OnPropertyChanged("EventExpander");
			}
		}
		
		/// <summary>
		/// Unique identifier of the associated event handler.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("eventhandler")]
		public Microsoft.Xrm.Sdk.EntityReference EventHandler
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("eventhandler");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("EventHandler");
				this.SetAttributeValue("eventhandler", value);
				this.OnPropertyChanged("EventHandler");
			}
		}
		
		/// <summary>
		/// Comma-separated list of attributes. If at least one of these attributes is modified, the plug-in should execute.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("filteringattributes")]
		public string FilteringAttributes
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("filteringattributes");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("FilteringAttributes");
				this.SetAttributeValue("filteringattributes", value);
				this.OnPropertyChanged("FilteringAttributes");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user to impersonate context when step is executed.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("impersonatinguserid")]
		public Microsoft.Xrm.Sdk.EntityReference ImpersonatingUserId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("impersonatinguserid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ImpersonatingUserId");
				this.SetAttributeValue("impersonatinguserid", value);
				this.OnPropertyChanged("ImpersonatingUserId");
			}
		}
		
		/// <summary>
		/// Version in which the form is introduced.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("introducedversion")]
		public string IntroducedVersion
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("introducedversion");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("IntroducedVersion");
				this.SetAttributeValue("introducedversion", value);
				this.OnPropertyChanged("IntroducedVersion");
			}
		}
		
		/// <summary>
		/// Identifies if a plug-in should be executed from a parent pipeline, a child pipeline, or both.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("invocationsource")]
		[System.ObsoleteAttribute()]
		public Microsoft.Xrm.Sdk.OptionSetValue InvocationSource
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("invocationsource");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("InvocationSource");
				this.SetAttributeValue("invocationsource", value);
				this.OnPropertyChanged("InvocationSource");
			}
		}
		
		/// <summary>
		/// Information that specifies whether this component can be customized.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("iscustomizable")]
		public Microsoft.Xrm.Sdk.BooleanManagedProperty IsCustomizable
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.BooleanManagedProperty>("iscustomizable");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("IsCustomizable");
				this.SetAttributeValue("iscustomizable", value);
				this.OnPropertyChanged("IsCustomizable");
			}
		}
		
		/// <summary>
		/// Information that specifies whether this component should be hidden.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ishidden")]
		public Microsoft.Xrm.Sdk.BooleanManagedProperty IsHidden
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.BooleanManagedProperty>("ishidden");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("IsHidden");
				this.SetAttributeValue("ishidden", value);
				this.OnPropertyChanged("IsHidden");
			}
		}
		
		/// <summary>
		/// Information that specifies whether this component is managed.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("ismanaged")]
		public System.Nullable<bool> IsManaged
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<bool>>("ismanaged");
			}
		}
		
		/// <summary>
		/// Run-time mode of execution, for example, synchronous or asynchronous.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("mode")]
		public virtual SdkMessageProcessingStep_Mode? Mode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return ((SdkMessageProcessingStep_Mode?)(EntityOptionSetEnum.GetEnum(this, "mode")));
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Mode");
				this.SetAttributeValue("mode", value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null);
				this.OnPropertyChanged("Mode");
			}
		}
		
		/// <summary>
		/// Unique identifier of the user who last modified the SDK message processing step.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedby")]
		public Microsoft.Xrm.Sdk.EntityReference ModifiedBy
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedby");
			}
		}
		
		/// <summary>
		/// Date and time when the SDK message processing step was last modified.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedon")]
		public System.Nullable<System.DateTime> ModifiedOn
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("modifiedon");
			}
		}
		
		/// <summary>
		/// Unique identifier of the delegate user who last modified the sdkmessageprocessingstep.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("modifiedonbehalfby")]
		public Microsoft.Xrm.Sdk.EntityReference ModifiedOnBehalfBy
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("modifiedonbehalfby");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("ModifiedOnBehalfBy");
				this.SetAttributeValue("modifiedonbehalfby", value);
				this.OnPropertyChanged("ModifiedOnBehalfBy");
			}
		}
		
		/// <summary>
		/// Name of SdkMessage processing step.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("name")]
		public string Name
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<string>("name");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Name");
				this.SetAttributeValue("name", value);
				this.OnPropertyChanged("Name");
			}
		}
		
		/// <summary>
		/// Unique identifier of the organization with which the SDK message processing step is associated.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("organizationid")]
		public Microsoft.Xrm.Sdk.EntityReference OrganizationId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("organizationid");
			}
		}
		
		/// <summary>
		/// For internal use only.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("overwritetime")]
		public System.Nullable<System.DateTime> OverwriteTime
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.DateTime>>("overwritetime");
			}
		}
		
		/// <summary>
		/// Unique identifier of the plug-in type associated with the step.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("plugintypeid")]
		[System.ObsoleteAttribute()]
		public Microsoft.Xrm.Sdk.EntityReference PluginTypeId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("plugintypeid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("PluginTypeId");
				this.SetAttributeValue("plugintypeid", value);
				this.OnPropertyChanged("PluginTypeId");
			}
		}
		
		/// <summary>
		/// Processing order within the stage.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("rank")]
		public System.Nullable<int> Rank
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<int>>("rank");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Rank");
				this.SetAttributeValue("rank", value);
				this.OnPropertyChanged("Rank");
			}
		}
		
		/// <summary>
		/// Unique identifier of the SDK message filter.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessagefilterid")]
		public Microsoft.Xrm.Sdk.EntityReference SdkMessageFilterId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("sdkmessagefilterid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("SdkMessageFilterId");
				this.SetAttributeValue("sdkmessagefilterid", value);
				this.OnPropertyChanged("SdkMessageFilterId");
			}
		}
		
		/// <summary>
		/// Unique identifier of the SDK message.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageid")]
		public Microsoft.Xrm.Sdk.EntityReference SdkMessageId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("sdkmessageid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("SdkMessageId");
				this.SetAttributeValue("sdkmessageid", value);
				this.OnPropertyChanged("SdkMessageId");
			}
		}
		
		/// <summary>
		/// Unique identifier of the SDK message processing step entity.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageprocessingstepid")]
		public System.Nullable<System.Guid> SdkMessageProcessingStepId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("sdkmessageprocessingstepid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("SdkMessageProcessingStepId");
				this.SetAttributeValue("sdkmessageprocessingstepid", value);
				if (value.HasValue)
				{
					base.Id = value.Value;
				}
				else
				{
					base.Id = System.Guid.Empty;
				}
				this.OnPropertyChanged("SdkMessageProcessingStepId");
			}
		}
		
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageprocessingstepid")]
		public override System.Guid Id
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return base.Id;
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.SdkMessageProcessingStepId = value;
			}
		}
		
		/// <summary>
		/// Unique identifier of the SDK message processing step.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageprocessingstepidunique")]
		public System.Nullable<System.Guid> SdkMessageProcessingStepIdUnique
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("sdkmessageprocessingstepidunique");
			}
		}
		
		/// <summary>
		/// Unique identifier of the Sdk message processing step secure configuration.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageprocessingstepsecureconfigid")]
		public Microsoft.Xrm.Sdk.EntityReference SdkMessageProcessingStepSecureConfigId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<Microsoft.Xrm.Sdk.EntityReference>("sdkmessageprocessingstepsecureconfigid");
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("SdkMessageProcessingStepSecureConfigId");
				this.SetAttributeValue("sdkmessageprocessingstepsecureconfigid", value);
				this.OnPropertyChanged("SdkMessageProcessingStepSecureConfigId");
			}
		}
		
		/// <summary>
		/// Unique identifier of the associated solution.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("solutionid")]
		public System.Nullable<System.Guid> SolutionId
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<System.Guid>>("solutionid");
			}
		}
		
		/// <summary>
		/// Stage in the execution pipeline that the SDK message processing step is in.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("stage")]
		public virtual SdkMessageProcessingStep_Stage? Stage
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return ((SdkMessageProcessingStep_Stage?)(EntityOptionSetEnum.GetEnum(this, "stage")));
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("Stage");
				this.SetAttributeValue("stage", value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null);
				this.OnPropertyChanged("Stage");
			}
		}
		
		/// <summary>
		/// Status of the SDK message processing step.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statecode")]
		public System.Nullable<CloudAwesome.Xrm.Customisation.SdkMessageProcessingStepState> StateCode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				Microsoft.Xrm.Sdk.OptionSetValue optionSet = this.GetAttributeValue<Microsoft.Xrm.Sdk.OptionSetValue>("statecode");
				if ((optionSet != null))
				{
					return ((CloudAwesome.Xrm.Customisation.SdkMessageProcessingStepState)(System.Enum.ToObject(typeof(CloudAwesome.Xrm.Customisation.SdkMessageProcessingStepState), optionSet.Value)));
				}
				else
				{
					return null;
				}
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("StateCode");
				if ((value == null))
				{
					this.SetAttributeValue("statecode", null);
				}
				else
				{
					this.SetAttributeValue("statecode", new Microsoft.Xrm.Sdk.OptionSetValue(((int)(value))));
				}
				this.OnPropertyChanged("StateCode");
			}
		}
		
		/// <summary>
		/// Reason for the status of the SDK message processing step.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("statuscode")]
		public virtual SdkMessageProcessingStep_StatusCode? StatusCode
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return ((SdkMessageProcessingStep_StatusCode?)(EntityOptionSetEnum.GetEnum(this, "statuscode")));
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("StatusCode");
				this.SetAttributeValue("statuscode", value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null);
				this.OnPropertyChanged("StatusCode");
			}
		}
		
		/// <summary>
		/// Deployment that the SDK message processing step should be executed on; server, client, or both.
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("supporteddeployment")]
		public virtual SdkMessageProcessingStep_SupportedDeployment? SupportedDeployment
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return ((SdkMessageProcessingStep_SupportedDeployment?)(EntityOptionSetEnum.GetEnum(this, "supporteddeployment")));
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("SupportedDeployment");
				this.SetAttributeValue("supporteddeployment", value.HasValue ? new Microsoft.Xrm.Sdk.OptionSetValue((int)value) : null);
				this.OnPropertyChanged("SupportedDeployment");
			}
		}
		
		/// <summary>
		/// Number that identifies a specific revision of the SDK message processing step. 
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("versionnumber")]
		public System.Nullable<long> VersionNumber
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetAttributeValue<System.Nullable<long>>("versionnumber");
			}
		}
		
		/// <summary>
		/// 1:N sdkmessageprocessingstepid_sdkmessageprocessingstepimage
		/// </summary>
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("sdkmessageprocessingstepid_sdkmessageprocessingstepimage")]
		public System.Collections.Generic.IEnumerable<CloudAwesome.Xrm.Customisation.SdkMessageProcessingStepImage> sdkmessageprocessingstepid_sdkmessageprocessingstepimage
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntities<CloudAwesome.Xrm.Customisation.SdkMessageProcessingStepImage>("sdkmessageprocessingstepid_sdkmessageprocessingstepimage", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("sdkmessageprocessingstepid_sdkmessageprocessingstepimage");
				this.SetRelatedEntities<CloudAwesome.Xrm.Customisation.SdkMessageProcessingStepImage>("sdkmessageprocessingstepid_sdkmessageprocessingstepimage", null, value);
				this.OnPropertyChanged("sdkmessageprocessingstepid_sdkmessageprocessingstepimage");
			}
		}
		
		/// <summary>
		/// N:1 plugintype_sdkmessageprocessingstep
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("eventhandler")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("plugintype_sdkmessageprocessingstep")]
		public CloudAwesome.Xrm.Customisation.PluginType plugintype_sdkmessageprocessingstep
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<CloudAwesome.Xrm.Customisation.PluginType>("plugintype_sdkmessageprocessingstep", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("plugintype_sdkmessageprocessingstep");
				this.SetRelatedEntity<CloudAwesome.Xrm.Customisation.PluginType>("plugintype_sdkmessageprocessingstep", null, value);
				this.OnPropertyChanged("plugintype_sdkmessageprocessingstep");
			}
		}
		
		/// <summary>
		/// N:1 plugintypeid_sdkmessageprocessingstep
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("plugintypeid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("plugintypeid_sdkmessageprocessingstep")]
		public CloudAwesome.Xrm.Customisation.PluginType plugintypeid_sdkmessageprocessingstep
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<CloudAwesome.Xrm.Customisation.PluginType>("plugintypeid_sdkmessageprocessingstep", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("plugintypeid_sdkmessageprocessingstep");
				this.SetRelatedEntity<CloudAwesome.Xrm.Customisation.PluginType>("plugintypeid_sdkmessageprocessingstep", null, value);
				this.OnPropertyChanged("plugintypeid_sdkmessageprocessingstep");
			}
		}
		
		/// <summary>
		/// N:1 sdkmessagefilterid_sdkmessageprocessingstep
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessagefilterid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("sdkmessagefilterid_sdkmessageprocessingstep")]
		public CloudAwesome.Xrm.Customisation.SdkMessageFilter sdkmessagefilterid_sdkmessageprocessingstep
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<CloudAwesome.Xrm.Customisation.SdkMessageFilter>("sdkmessagefilterid_sdkmessageprocessingstep", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("sdkmessagefilterid_sdkmessageprocessingstep");
				this.SetRelatedEntity<CloudAwesome.Xrm.Customisation.SdkMessageFilter>("sdkmessagefilterid_sdkmessageprocessingstep", null, value);
				this.OnPropertyChanged("sdkmessagefilterid_sdkmessageprocessingstep");
			}
		}
		
		/// <summary>
		/// N:1 sdkmessageid_sdkmessageprocessingstep
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("sdkmessageid_sdkmessageprocessingstep")]
		public CloudAwesome.Xrm.Customisation.SdkMessage sdkmessageid_sdkmessageprocessingstep
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<CloudAwesome.Xrm.Customisation.SdkMessage>("sdkmessageid_sdkmessageprocessingstep", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("sdkmessageid_sdkmessageprocessingstep");
				this.SetRelatedEntity<CloudAwesome.Xrm.Customisation.SdkMessage>("sdkmessageid_sdkmessageprocessingstep", null, value);
				this.OnPropertyChanged("sdkmessageid_sdkmessageprocessingstep");
			}
		}
		
		/// <summary>
		/// N:1 sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("sdkmessageprocessingstepsecureconfigid")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep")]
		public CloudAwesome.Xrm.Customisation.SdkMessageProcessingStepSecureConfig sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<CloudAwesome.Xrm.Customisation.SdkMessageProcessingStepSecureConfig>("sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep");
				this.SetRelatedEntity<CloudAwesome.Xrm.Customisation.SdkMessageProcessingStepSecureConfig>("sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep", null, value);
				this.OnPropertyChanged("sdkmessageprocessingstepsecureconfigid_sdkmessageprocessingstep");
			}
		}
		
		/// <summary>
		/// N:1 serviceendpoint_sdkmessageprocessingstep
		/// </summary>
		[Microsoft.Xrm.Sdk.AttributeLogicalNameAttribute("eventhandler")]
		[Microsoft.Xrm.Sdk.RelationshipSchemaNameAttribute("serviceendpoint_sdkmessageprocessingstep")]
		public CloudAwesome.Xrm.Customisation.ServiceEndpoint serviceendpoint_sdkmessageprocessingstep
		{
			[System.Diagnostics.DebuggerNonUserCode()]
			get
			{
				return this.GetRelatedEntity<CloudAwesome.Xrm.Customisation.ServiceEndpoint>("serviceendpoint_sdkmessageprocessingstep", null);
			}
			[System.Diagnostics.DebuggerNonUserCode()]
			set
			{
				this.OnPropertyChanging("serviceendpoint_sdkmessageprocessingstep");
				this.SetRelatedEntity<CloudAwesome.Xrm.Customisation.ServiceEndpoint>("serviceendpoint_sdkmessageprocessingstep", null, value);
				this.OnPropertyChanged("serviceendpoint_sdkmessageprocessingstep");
			}
		}
		
		/// <summary>
		/// Constructor for populating via LINQ queries given a LINQ anonymous type
		/// <param name="anonymousType">LINQ anonymous type.</param>
		/// </summary>
		[System.Diagnostics.DebuggerNonUserCode()]
		public SdkMessageProcessingStep(object anonymousType) : 
				this()
		{
            foreach (var p in anonymousType.GetType().GetProperties())
            {
                var value = p.GetValue(anonymousType, null);
                var name = p.Name.ToLower();
            
                if (name.EndsWith("enum") && value.GetType().BaseType == typeof(System.Enum))
                {
                    value = new Microsoft.Xrm.Sdk.OptionSetValue((int) value);
                    name = name.Remove(name.Length - "enum".Length);
                }
            
                switch (name)
                {
                    case "id":
                        base.Id = (System.Guid)value;
                        Attributes["sdkmessageprocessingstepid"] = base.Id;
                        break;
                    case "sdkmessageprocessingstepid":
                        var id = (System.Nullable<System.Guid>) value;
                        if(id == null){ continue; }
                        base.Id = id.Value;
                        Attributes[name] = base.Id;
                        break;
                    case "formattedvalues":
                        // Add Support for FormattedValues
                        FormattedValues.AddRange((Microsoft.Xrm.Sdk.FormattedValueCollection)value);
                        break;
                    default:
                        Attributes[name] = value;
                        break;
                }
            }
		}
	}
}