using CloudAwesome.Xrm.Customisation.Models;
using FluentValidation;

namespace CloudAwesome.Xrm.Customisation.ModelValidators
{
    public class CdsAttributeValidator: AbstractValidator<CdsAttribute>
    {
        public CdsAttributeValidator()
        {
            RuleFor(attribute => attribute.DisplayName)
                .NotEmpty()
                .WithMessage(attribute => $"Display name is always a mandatory field. " +
                                          $"Check for any attributes with a missing display name");
            
            When(attribute => attribute.DataType == CdsAttributeDataType.Memo, () =>
            {
                RuleFor(attribute => attribute.MaxLength)
                    .NotEmpty()
                    .WithMessage(attribute => $"{attribute.DisplayName}: " +
                                              $"Max length is a mandatory field for multi-line text fields");
            });

            // When(attribute => attribute.DataType == CdsAttributeDataType.String, () =>
            // {
            //
            // });
            
            // When(attribute => attribute.DataType == CdsAttributeDataType.Integer, () =>
            // {
            //
            // });
            //
            // When(attribute => attribute.DataType == CdsAttributeDataType.Picklist, () =>
            // {
            //
            // });
            //
            // When(attribute => attribute.DataType == CdsAttributeDataType.DateTime, () =>
            // {
            //
            // });
            //
            // When(attribute => attribute.DataType == CdsAttributeDataType.Boolean, () =>
            // {
            //
            // });
            //
            // When(attribute => attribute.DataType == CdsAttributeDataType.Lookup, () =>
            // {
            //
            // });
            
        }
        
    }
}