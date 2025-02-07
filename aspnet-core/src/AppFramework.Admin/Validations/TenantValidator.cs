using Abp.Authorization.Users;
using Abp.MultiTenancy;
using AppFramework.MultiTenancy;
using AppFramework.MultiTenancy.Dto;
using AppFramework.Shared.Validations;
using FluentValidation;

namespace AppFramework.Admin.Validations
{
    public class CreateTenantValidator : AbstractValidator<CreateTenantInput>
    {
        public CreateTenantValidator()
        {
            RuleFor(x => x.TenancyName).IsRequired().Regular(TenantConsts.TenancyNameRegex).MaxLength(AbpTenantBase.MaxTenancyNameLength);
            RuleFor(x => x.Name).IsRequired().MaxLength(AbpTenantBase.MaxNameLength);
            RuleFor(x => x.AdminEmailAddress).IsRequired().Email().MaxLength(AbpUserBase.MaxEmailAddressLength);
            RuleFor(x => x.AdminPassword).MaxLength(AbpUserBase.MaxPasswordLength);
            RuleFor(x => x.ConnectionString).MaxLength(AbpTenantBase.MaxConnectionStringLength);
        }
    }

    public class UpdateTenantValidator : AbstractValidator<TenantEditDto>
    {
        public UpdateTenantValidator()
        {
            RuleFor(x => x.TenancyName).IsRequired().Regular(TenantConsts.TenancyNameRegex).MaxLength(AbpTenantBase.MaxTenancyNameLength);
            RuleFor(x => x.Name).IsRequired().MaxLength(AbpTenantBase.MaxNameLength);
        }
    }
}