namespace CloudAwesome.Xrm.Customisation.Models
{
    public class CdsPluginAssembly
    {
        public string Name { get; set; }

        public string FriendlyName { get; set; }

        public string Assembly { get; set; }

        public string SolutionName { get; set; }

        public CdsPlugin[] Plugins { get; set; }
    }
}
