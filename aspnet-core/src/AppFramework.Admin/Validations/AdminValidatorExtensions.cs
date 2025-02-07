using AppFramework.Admin.Models;
using AppFramework.MultiTenancy.Dto;
using AppFramework.Shared.Models.Configuration;
using AppFramework.Admin.Validations;
using FluentValidation;
using Prism.Ioc; 

namespace AppFramework.Admin
{
    public static class AdminValidatorExtensions
    { 
        public static void AddValidators(this IContainerRegistry services)
        { 
            services.RegisterScoped<IValidator<UserCreateOrUpdateModel>, UserCreateOrUpdateValidator>();
            services.RegisterScoped<IValidator<CreateOrganizationUnitModel>, OrganizationUnitValidator>();
            services.RegisterScoped<IValidator<CreateTenantInput>, CreateTenantValidator>();
            services.RegisterScoped<IValidator<TenantEditDto>, UpdateTenantValidator>();
            services.RegisterScoped<IValidator<EditionCreateModel>, CreateEditionValidator>();
            services.RegisterScoped<IValidator<HostSettingsEditModel>, SettingsValidator>();
            services.RegisterScoped<IValidator<VersionListModel>, VersionValidator>();
        }
    }
}
