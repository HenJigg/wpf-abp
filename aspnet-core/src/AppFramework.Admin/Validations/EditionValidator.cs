using AppFramework.Admin.Models;
using AppFramework.Shared.Validations;
using FluentValidation;

namespace AppFramework.Admin.Validations
{
    public class CreateEditionValidator : AbstractValidator<EditionCreateModel>
    {
        public CreateEditionValidator()
        {
            RuleFor(x => x.DisplayName).IsRequired();
        }
    }
}