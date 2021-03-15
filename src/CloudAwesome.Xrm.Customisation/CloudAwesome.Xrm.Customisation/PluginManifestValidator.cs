using CloudAwesome.Xrm.Customisation.Validators;
using FluentValidation;

namespace CloudAwesome.Xrm.Customisation
{
    public class PluginManifestValidator: AbstractValidator<PluginManifest>
    {
        public PluginManifestValidator()
        {
            RuleForEach(manifest => manifest.PluginAssemblies).SetValidator(new CdsPluginAssemblyValidator());
        }
    }
}