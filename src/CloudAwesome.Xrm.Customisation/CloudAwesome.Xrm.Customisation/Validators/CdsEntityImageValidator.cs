using CloudAwesome.Xrm.Customisation.Models;
using FluentValidation;

namespace CloudAwesome.Xrm.Customisation.Validators
{
    public class CdsEntityImageValidator: AbstractValidator<CdsEntityImage>
    {
        public CdsEntityImageValidator()
        {
            RuleFor(image => image.Name).NotNull();
            RuleFor(image => image.Attributes).NotEmpty();
        }
    }
}