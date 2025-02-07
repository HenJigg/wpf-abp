using System.ComponentModel.DataAnnotations;

namespace AppFramework.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
