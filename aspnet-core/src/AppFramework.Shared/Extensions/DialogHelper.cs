using Hardcodet.Wpf.TaskbarNotification;

namespace AppFramework.Shared
{
    public static class DialogHelper
    {
        private static IAppTaskBar taskBar;

        static DialogHelper()
        {
            if (System.Windows.Application.Current is IAppTaskBar appTaskBar)
                taskBar = appTaskBar;
        }

        public static void Info(string title, string message)
        {
            taskBar.ShowBalloonTip(title, message, BalloonIcon.Info);
        }

        public static void Error(string title, string message)
        {
            taskBar.ShowBalloonTip(title, message, BalloonIcon.Error);
        }

        public static void Warning(string title, string message)
        {
            taskBar.ShowBalloonTip(title, message, BalloonIcon.Warning);
        }
    }
}
