namespace CloudAwesome.Xrm.Customisation.Models
{
    public enum ApiBindingType { Global, Entity, EntityCollection }

    public enum CustomProcessingStepType { None, AsyncOnly, SyncAndAsync }

    public class CdsCustomApi
    {
        public string Name { get; set; }

        public string FriendlyName { get; set; }

        public string Description { get; set; }

        public string ExecutePrivilegeName { get; set; }

        public bool IsFunction { get; set; }

        public bool IsPrivate { get; set; }

        public CustomProcessingStepType AllowedCustomProcessingStepType { get; set; }

        public ApiBindingType BindingType { get; set; }

        public CdsRequestParameter[] RequestParameters { get; set; }

        public CdsResponseProperty[] ResponseProperties { get; set; }

    }
}
