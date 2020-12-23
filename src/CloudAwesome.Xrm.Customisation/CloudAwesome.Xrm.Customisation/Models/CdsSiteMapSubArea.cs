namespace CloudAwesome.Xrm.Customisation.Models
{
    public enum CdsSiteMapSubAreaType { Entity }

    public class CdsSiteMapSubArea
    {
        public CdsSiteMapSubAreaType Type { get; set; }
        public string Entity { get; set; }
        public string Title { get; set; }
    }
}
