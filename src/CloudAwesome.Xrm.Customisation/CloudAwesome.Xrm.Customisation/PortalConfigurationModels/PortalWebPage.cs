namespace CloudAwesome.Xrm.Customisation.PortalConfigurationModels
{
    public class PortalWebPage: BasePortalConfigurationEntity, IPortalConfigurationEntity
    {
        public string Name { get; set; }
        
        public string ParentPage { get; set; }
        
        public string PageTemplate { get; set; }
        
        public string EntityList { get; set; }
        
        public string EntityForm { get; set; }
        
        public string Description { get; set; }
        
        public string Title { get; set; }
        
        public string Summary { get; set; }
        
        public string Copy { get; set; }
        
        public string Navigation { get; set; }
    }
}