using System.Diagnostics.CodeAnalysis;

namespace CloudAwesome.Xrm.Customisation.DocumentationGenerator
{
    public class PortalDocumentationGenerator
    {
        [ExcludeFromCodeCoverage] // Alpha dev. Parked for now
        public void GenerateDocumentationFromWebsite()
        {
            
        }

        public void GenerateDocumentationFromParentWebPage()
        {
            
        }

        public void GeneratePortalSecurityModelDocumentation()
        {
            
        }

        public void GenerateSiteMapDiagram(string parentPage)
        {
            
        }
        
        public void GenerateSiteMapDiagram()
        {
            GenerateSiteMapDiagram("Home");
        }
    }
}