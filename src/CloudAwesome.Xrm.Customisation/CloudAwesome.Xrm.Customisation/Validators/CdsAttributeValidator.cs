using CloudAwesome.Xrm.Customisation.Models;
using FluentValidation;

namespace CloudAwesome.Xrm.Customisation.Validators
{
    public class CdsAttributeValidator: AbstractValidator<CdsAttribute>
    {
        public CdsAttributeValidator()
        {
            RuleFor(attribute => attribute.DisplayName)
                .NotEmpty()
                .WithMessage(attribute => $"Display name is always a mandatory field. Check for any attributes with a missing display name");

            When(attribute => attribute.DataType == CdsAttributeDataType.Memo, () =>
            {
                RuleFor(attribute => attribute.MaxLength)
                    .NotEmpty()
                    .WithMessage(attribute => $"{attribute.DisplayName}: Max length is a mandatory field for multi-line text fields");
            });
        }
        
    }
}