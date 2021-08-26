using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace CloudAwesome.Xrm.Customisation
{
    public static class EnumExtensions
    {
        public static T GetEnumMemberValue<T>(this Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
        
    }
}