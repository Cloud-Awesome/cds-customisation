using CloudAwesome.Xrm.Customisation.Models;
using FluentValidation;

namespace CloudAwesome.Xrm.Customisation.Validators
{
    public class CdsEntityValidator: AbstractValidator<CdsEntity>
    {
        public CdsEntityValidator()
        {
            RuleFor(entity => entity.DisplayName).NotEmpty();

            When(entity => entity.IsActivity != null && entity.IsActivity.Value, () =>
            {
                
            });

            RuleForEach(entity => entity.Attributes).SetValidator(new CdsAttributeValidator());
        }
    }
}