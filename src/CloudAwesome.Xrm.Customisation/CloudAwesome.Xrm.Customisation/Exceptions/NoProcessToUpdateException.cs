using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudAwesome.Xrm.Customisation.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class NoProcessToUpdateException: Exception
    {
        public NoProcessToUpdateException()
        {

        }

        public NoProcessToUpdateException(string message) : base(message)
        {

        }

        public NoProcessToUpdateException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
