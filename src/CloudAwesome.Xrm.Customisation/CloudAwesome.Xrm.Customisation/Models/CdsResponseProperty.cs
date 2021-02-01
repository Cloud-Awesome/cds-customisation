namespace CloudAwesome.Xrm.Customisation.Models
{
    public class CdsResponseProperty
    {
        public string Name { get; set; }

        public string FriendlyName { get; set; }

        public string Description { get; set; }

        public string Type { get; set; } // <- TODO - this is an enum, get the values ;)
    }
}