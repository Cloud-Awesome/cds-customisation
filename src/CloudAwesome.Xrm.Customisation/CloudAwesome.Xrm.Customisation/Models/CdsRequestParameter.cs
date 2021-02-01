namespace CloudAwesome.Xrm.Customisation.Models
{
    public class CdsRequestParameter
    {
        public string Name { get; set; }

        public string FriendlyName { get; set; }

        public string Description { get; set; }

        public string LogicalEntityName { get; set; }

        public bool IsOptional { get; set; }
        
        public string Type { get; set; } // <- TODO - this is an enum, get the values
    }
}