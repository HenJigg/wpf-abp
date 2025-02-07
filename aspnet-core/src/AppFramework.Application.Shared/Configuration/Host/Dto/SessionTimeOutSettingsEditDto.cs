using System.ComponentModel.DataAnnotations;

namespace AppFramework.Configuration.Host.Dto
{
    public class SessionTimeOutSettingsEditDto
    {
        public bool IsEnabled { get; set; }

        [Range(10, int.MaxValue)]
        public int TimeOutSecond { get; set; }

        [Range(10, int.MaxValue)]
        public int ShowTimeOutNotificationSecond { get; set; }

        public bool ShowLockScreenWhenTimedOut { get; set; }
    }
}
