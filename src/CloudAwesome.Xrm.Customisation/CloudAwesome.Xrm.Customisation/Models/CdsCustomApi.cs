namespace CloudAwesome.Xrm.Customisation.Models
{
    public class CdsCustomApi
    {
        public string Name { get; set; }

        public string FriendlyName { get; set; }

        public string Description { get; set; }

        public string ExecutePrivilegeName { get; set; } // <- TODO - what's this? :) A string for rolepriv?

        public bool IsFunction { get; set; }

        public bool IsPrivate { get; set; }

        public string AllowedCustomProcessingStepType { get; set; } // <- TODO - this is an enum?

        public string BindingType { get; set; } // <- TODO - this is an enum?

        public CdsRequestParameter[] RequestParameters { get; set; }

        public CdsResponseProperty[] ResponseProperties { get; set; }

    }
}
