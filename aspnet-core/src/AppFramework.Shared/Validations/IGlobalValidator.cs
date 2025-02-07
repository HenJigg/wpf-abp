using FluentValidation.Results;

namespace AppFramework.Shared.Validations
{
    public interface IGlobalValidator
    {
        ValidationResult Validate<T>(T model);
    }
}