using Hardcodet.Wpf.TaskbarNotification;
using System.Windows.Forms;

namespace AppFramework.Shared
{
    public interface IAppTaskBar
    {
        void Initialization();

        void Dispose();

        void ShowBalloonTip(string title, string message, BalloonIcon balloonIcon);
    }
}
