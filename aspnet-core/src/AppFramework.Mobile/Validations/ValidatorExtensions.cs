using AppFramework.Shared.Models;
using AppFramework.Shared.Models.Configuration;
using AppFramework.MultiTenancy.Dto;
using FluentValidation;
using FluentValidation.Validators;
using Prism.Ioc;
using AppFramework.Shared.Extensions;

namespace AppFramework.Shared.Validations
{
    public static class ValidatorExtensions
    {
        /// <summary>
        ///  注册FluentValidation
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterValidator(this IContainerRegistry services)
        {
            services.RegisterSingleton<IGlobalValidator, GlobalValidator>();
            services.RegisterScoped<IValidator<UserCreateOrUpdateModel>, UserCreateOrUpdateValidator>();
            services.RegisterScoped<IValidator<CreateOrganizationUnitModel>, OrganizationUnitValidator>();
            services.RegisterScoped<IValidator<CreateTenantInput>, CreateTenantValidator>();
            services.RegisterScoped<IValidator<TenantEditDto>, UpdateTenantValidator>();
            services.RegisterScoped<IValidator<EditionCreateModel>, CreateEditionValidator>();
            services.RegisterScoped<IValidator<HostSettingsEditModel>, SettingsValidator>();
            services.RegisterScoped<IValidator<VersionListModel>, VersionValidator>();
        }

        #region 验证器扩展

        /// <summary>
        /// 属性属于必填项验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, TProperty> IsRequired<T, TProperty>(
            this IRuleBuilder<T, TProperty> ruleBuilder,
            string errorMessage = LocalizationKeys.RequiredField)
        {
            return ruleBuilder.SetValidator(new NotEmptyValidator<T, TProperty>()).WithMessage(errorMessage);
        }

        /// <summary>
        /// 最大长度验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="maximumLength"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> MaxLength<T>(
            this IRuleBuilder<T, string> ruleBuilder,
            int maximumLength,
            string errorMessage = LocalizationKeys.MoreThanMaxStringLength)
        {
            return ruleBuilder.SetValidator(new MaximumLengthValidator<T>(maximumLength)).WithMessage(errorMessage);
        }

        /// <summary>
        /// 最小长度验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="minimumLength"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> MinLength<T>(this IRuleBuilder<T, string> ruleBuilder,
            int minimumLength,
            string errorMessage = LocalizationKeys.LessThanMinStringLength)
        {
            return ruleBuilder.SetValidator(new MinimumLengthValidator<T>(minimumLength)).WithMessage(errorMessage);
        }

        /// <summary>
        /// 邮箱验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="mode"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> Email<T>(
           this IRuleBuilder<T, string> ruleBuilder,
           EmailValidationMode mode = EmailValidationMode.AspNetCoreCompatible,
           string errorMessage = LocalizationKeys.InvalidEmailAddress)
        {
#pragma warning disable 618
            var validator = mode == EmailValidationMode.AspNetCoreCompatible ?
                new AspNetCoreCompatibleEmailValidator<T>() :
                (PropertyValidator<T, string>)new EmailValidator<T>();
#pragma warning restore 618
            return ruleBuilder.SetValidator(validator).WithMessage(errorMessage);
        }

        /// <summary>
        /// 正则表达式验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ruleBuilder"></param>
        /// <param name="expression"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static IRuleBuilderOptions<T, string> Regular<T>(this IRuleBuilder<T, string> ruleBuilder,
            string expression,
            string errorMessage = LocalizationKeys.InvalidRegularExpression)
        {
            return ruleBuilder.SetValidator(new RegularExpressionValidator<T>(expression)).WithMessage(errorMessage);
        }

        #endregion
    }
}