using System.Collections.Generic;

namespace CloudAwesome.Xrm.Customisation.Models
{
    public class ManifestValidationResult
    {
        public bool IsValid { get; set; }
        
        public IEnumerable<string> Errors { get; set; }

        public override string ToString()
        {
            return string.Join("\n", Errors);
        }
    }
}