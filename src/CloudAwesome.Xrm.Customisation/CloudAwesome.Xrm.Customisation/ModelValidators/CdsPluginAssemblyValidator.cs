using CloudAwesome.Xrm.Customisation.Models;
using FluentValidation;

namespace CloudAwesome.Xrm.Customisation.ModelValidators
{
    public class CdsPluginAssemblyValidator: AbstractValidator<CdsPluginAssembly>
    {
        public CdsPluginAssemblyValidator()
        {
            RuleFor(assembly => assembly.Name).NotNull();
            RuleFor(assembly => assembly.FriendlyName).NotNull();
            RuleFor(assembly => assembly.Assembly).NotNull();

            RuleForEach(assembly => assembly.Plugins).SetValidator(new CdsPluginValidator());
        }
    }
}