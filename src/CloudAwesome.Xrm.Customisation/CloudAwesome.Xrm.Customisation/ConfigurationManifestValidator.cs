﻿using System.Linq;
using CloudAwesome.Xrm.Customisation.Models;
using CloudAwesome.Xrm.Customisation.Validators;
using FluentValidation;

namespace CloudAwesome.Xrm.Customisation
{
    public class ConfigurationManifestValidator: AbstractValidator<ConfigurationManifest>
    {
        public ConfigurationManifestValidator()
        {
            RuleFor(manifest => manifest.SolutionName)
                .NotEmpty()
                .Matches(@"\A\S+\z")
                .WithMessage("Solution name is mandatory and must not contain any spaces (i.e. use the schema name, not the friendly name)");
            
            RuleForEach(manifest => manifest.Entities)
                .SetValidator(new CdsEntityValidator());
            
            
        }
        
    }
}