using CloudAwesome.Xrm.Customisation.Models;
using FluentValidation;

namespace CloudAwesome.Xrm.Customisation.Validators
{
    public class CdsPluginValidator: AbstractValidator<CdsPlugin>
    {
        public CdsPluginValidator()
        {
            RuleFor(plugin => plugin.Name).NotNull();
            RuleFor(plugin => plugin.FriendlyName).NotNull();
            RuleForEach(plugin => plugin.Steps).SetValidator(new CdsPluginStepValidator());
        }
    }
}