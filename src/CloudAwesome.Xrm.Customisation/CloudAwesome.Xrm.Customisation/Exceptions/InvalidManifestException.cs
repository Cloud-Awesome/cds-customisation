using System;
using System.Diagnostics.CodeAnalysis;

namespace CloudAwesome.Xrm.Customisation.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class InvalidManifestException: Exception
    {
        public InvalidManifestException()
        {

        }

        public InvalidManifestException(string message) : base(message)
        {

        }

        public InvalidManifestException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}