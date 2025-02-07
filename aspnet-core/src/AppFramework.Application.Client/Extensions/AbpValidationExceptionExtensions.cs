using Abp.Collections.Extensions;
using Abp.Runtime.Validation;
using System;
using System.Linq;

namespace AppFramework.Extensions
{
    public static class AbpValidationExceptionExtensions
    {
        public static string GetConsolidatedMessage(this AbpValidationException abpValidationException)
        {
            if (abpValidationException?.ValidationErrors == null)
            {
                return null;
            }

            if (!abpValidationException.ValidationErrors.Any())
            {
                return null;
            }

            var validationErrorMessages = abpValidationException.ValidationErrors.Select(e => "* " + e.ErrorMessage);
            var consolidatedMessage = validationErrorMessages.JoinAsString(Environment.NewLine);
            return consolidatedMessage;
        }
    }
}