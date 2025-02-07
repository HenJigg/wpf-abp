using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppFramework.Authorization.Users.Delegation.Dto
{
    public class CreateUserDelegationDto : IValidatableObject
    {
        [Required]
        [Range(1, long.MaxValue)]
        public long TargetUserId { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartTime > EndTime)
            {
                yield return new ValidationResult("StartTime of a user delegation operation can't be bigger than EndTime!");
            }
        }
    }
}