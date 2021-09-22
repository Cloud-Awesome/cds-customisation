using CloudAwesome.Xrm.Customisation.Validators;
using FluentValidation;

namespace CloudAwesome.Xrm.Customisation.PluginRegistration
{
    public class PluginManifestValidator: AbstractValidator<PluginManifest>
    {
        public PluginManifestValidator()
        {
            RuleFor(manifest => manifest.CdsConnection).NotEmpty();
            When(manifest => manifest.PluginAssemblies != null, () =>
            {
                RuleForEach(manifest => manifest.PluginAssemblies)
                    .SetValidator(new CdsPluginAssemblyValidator());
            });
        }
    }
}