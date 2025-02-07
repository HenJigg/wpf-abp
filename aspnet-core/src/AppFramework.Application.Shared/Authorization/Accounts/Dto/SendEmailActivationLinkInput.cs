using System.ComponentModel.DataAnnotations;

namespace AppFramework.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}