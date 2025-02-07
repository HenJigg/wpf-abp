using Abp.Organizations;
using AppFramework.Admin.Models;
using AppFramework.Shared.Validations;
using FluentValidation;

namespace AppFramework.Admin.Validations
{
    public class OrganizationUnitValidator : AbstractValidator<CreateOrganizationUnitModel>
    {
        public OrganizationUnitValidator()
        {
            RuleFor(x => x.DisplayName).IsRequired().MaxLength(OrganizationUnit.MaxDisplayNameLength);
        }
    }
}