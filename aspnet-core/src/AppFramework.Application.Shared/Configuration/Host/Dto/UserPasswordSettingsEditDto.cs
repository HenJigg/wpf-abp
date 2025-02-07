namespace AppFramework.Configuration.Host.Dto
{
    public class UserPasswordSettingsEditDto
    {
        public bool EnableCheckingLastXPasswordWhenPasswordChange { get; set; }

        public int CheckingLastXPasswordCount { get; set; }
        
        public bool EnablePasswordExpiration { get; set; }

        public int PasswordExpirationDayCount { get; set; }
    }
}