using AppFramework.Shared;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Ioc; 
using AppFramework.Shared.Services.App;
using AppFramework.Shared.Services;

namespace AppFramework.Admin.ViewModels
{
    public class TaskBarViewModel : BindableBase
    {
        private readonly IHostDialogService dialog;
        private readonly IAppStartService appStartService;
        public DelegateCommand ExitCommand { get; set; }
        public DelegateCommand ShowViewCommand { get; private set; }

        public TaskBarViewModel()
        {
            dialog = ContainerLocator.Container.Resolve<IHostDialogService>();
            appStartService= ContainerLocator.Container.Resolve<IAppStartService>();

            ExitCommand = new DelegateCommand(Exit);
            ShowViewCommand = new DelegateCommand(ShowView);
        }

        private async void Exit()
        {
            ShowView();

            if (await dialog.Question(Local.Localize("AreYouSure")))
                appStartService.Exit();
        }

        private void ShowView()
        { 
            if (!System.Windows.Application.Current.MainWindow.IsVisible)
            {
                System.Windows.Application.Current.MainWindow.Show();
                System.Windows.Application.Current.MainWindow.WindowState = System.Windows.WindowState.Normal;
            }
        }
    }
}
