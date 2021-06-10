using Microsoft.Crm.Sdk.Messages;

namespace CloudAwesome.Xrm.Customisation.Models
{
    public class CdsEntityPermission
    {
        public string RoleName { get; set; }

        public PrivilegeDepth? Create { get; set; }

        public PrivilegeDepth? Read { get; set; }

        public PrivilegeDepth? Write { get; set; }

        public PrivilegeDepth? Delete { get; set; }

        public PrivilegeDepth? Append { get; set; }

        public PrivilegeDepth? AppendTo { get; set; }

        public PrivilegeDepth? Share { get; set; }
    }
}
