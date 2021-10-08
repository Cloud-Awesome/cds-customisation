using System;
using System.Diagnostics.CodeAnalysis;

namespace CloudAwesome.Xrm.Customisation.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NotCustomisableException: Exception
    {
        public NotCustomisableException()
        {

        }

        public NotCustomisableException(string message) : base(message)
        {

        }

        public NotCustomisableException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
