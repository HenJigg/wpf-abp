using System.Windows;

namespace AppFramework.Shared.Services.App
{
    public interface IAppStartService
    {
        void CreateShell();

        void Logout();

        void Exit();
    }
}
