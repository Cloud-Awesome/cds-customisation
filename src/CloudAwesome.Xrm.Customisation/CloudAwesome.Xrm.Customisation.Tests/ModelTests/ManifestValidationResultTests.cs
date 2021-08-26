using System.Collections.Generic;
using CloudAwesome.Xrm.Customisation.Models;
using FluentAssertions;
using NUnit.Framework;

namespace CloudAwesome.Xrm.Customisation.Tests.ModelTests
{
    
    public class ManifestValidationResultTests
    {
        [Test]
        public void Overriden_ToString_Returns_Errors_With_NewLine_Delimiter()
        {
            var validationResults = new ManifestValidationResult()
            {
                IsValid = true,
                Errors = new List<string>()
                {
                    "Error 1",
                    "Error 2",
                    "Error 3"
                }
            };

            validationResults.ToString().Should().Be("Error 1\nError 2\nError 3");
        }
    }
}