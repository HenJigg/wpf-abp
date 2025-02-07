using AppFramework.Shared.Models;
using AppFramework.Shared.Models.Configuration;
using AppFramework.MultiTenancy.Dto;
using FluentValidation;
using FluentValidation.Validators;
using Prism.Ioc;

namespace AppFramework.Shared.Validations
{
    public static class ValidatorExtensions
    { 
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
            string errorMessage = AppLocalizationKeys.RequiredField)
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
            string errorMessage = AppLocalizationKeys.MoreThanMaxStringLength)
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
            string errorMessage = AppLocalizationKeys.LessThanMinStringLength)
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
           string errorMessage = AppLocalizationKeys.InvalidEmailAddress)
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
            string errorMessage = AppLocalizationKeys.InvalidRegularExpression)
        {
            return ruleBuilder.SetValidator(new RegularExpressionValidator<T>(expression)).WithMessage(errorMessage);
        }

        #endregion
    }
}