namespace AppFramework.Configuration.Host.Dto
{
    public class UserLockOutSettingsEditDto
    {
        public bool IsEnabled { get; set; }

        public int MaxFailedAccessAttemptsBeforeLockout { get; set; }

        public int DefaultAccountLockoutSeconds { get; set; }
    }
}