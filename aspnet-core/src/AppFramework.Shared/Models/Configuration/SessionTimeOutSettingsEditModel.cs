using Prism.Mvvm;
using System.ComponentModel.DataAnnotations;

namespace AppFramework.Shared.Models.Configuration
{
    public class SessionTimeOutSettingsEditModel : BindableBase
    {
        private bool isEnabled;
        private int timeOutSecond;
        private int showTimeOutNotificationSecond;
        private bool showLockScreenWhenTimedOut;

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; RaisePropertyChanged(); }
        }

        [Range(10, int.MaxValue)]
        public int TimeOutSecond
        {
            get { return timeOutSecond; }
            set { timeOutSecond = value; RaisePropertyChanged(); }
        }

        [Range(10, int.MaxValue)]
        public int ShowTimeOutNotificationSecond
        {
            get { return showTimeOutNotificationSecond; }
            set { showTimeOutNotificationSecond = value; RaisePropertyChanged(); }
        }

        public bool ShowLockScreenWhenTimedOut
        {
            get { return showLockScreenWhenTimedOut; }
            set { showLockScreenWhenTimedOut = value; RaisePropertyChanged(); }
        }
    }
}