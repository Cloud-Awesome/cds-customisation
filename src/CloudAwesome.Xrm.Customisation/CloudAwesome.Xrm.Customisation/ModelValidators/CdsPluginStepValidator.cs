using CloudAwesome.Xrm.Customisation.Models;
using FluentValidation;

namespace CloudAwesome.Xrm.Customisation.ModelValidators
{
    public class CdsPluginStepValidator: AbstractValidator<CdsPluginStep>
    {
        public CdsPluginStepValidator()
        {
            RuleFor(step => step.Name).NotNull();
            RuleFor(step => step.FriendlyName).NotNull();
            RuleFor(step => step.Stage).IsInEnum();
            RuleFor(step => step.Message).NotNull();
            RuleFor(step => step.PrimaryEntity).NotNull();
            RuleForEach(step => step.EntityImages).SetValidator(new CdsEntityImageValidator());
        }
    }
}