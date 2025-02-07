using FluentValidation;
using FluentValidation.Results;
using Prism.Ioc;
using System;

namespace AppFramework.Shared.Validations
{
    public class GlobalValidator : ValidatorFactoryBase, IGlobalValidator
    {
        private readonly IContainerProvider provider;

        public GlobalValidator(IContainerProvider provider)
        {
            this.provider=provider;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            return provider.Resolve(validatorType) as IValidator;
        }

        public ValidationResult Validate<T>(T model)
        {
            return GetValidator<T>().Validate(model);
        }
    }
}