namespace AppFramework.Shared.ViewModels
{
    using Prism.Regions.Navigation;
    using System;
    using System.Threading.Tasks;
    using Prism.Ioc;
    using Prism.Services.Dialogs;
    using Prism.Navigation;

    public class RegionViewModel : ViewModelBase, IRegionAware
    {
        private readonly IUserDialogService applayer;
        public readonly IDialogService dialogService;
        public readonly INavigationService navigationService;

        public RegionViewModel()
        {
            applayer = ContainerLocator.Container.Resolve<IUserDialogService>();
            dialogService = ContainerLocator.Container.Resolve<IDialogService>();
            navigationService = ContainerLocator.Container.Resolve<INavigationService>();
        }

        public virtual bool IsNavigationTarget(INavigationContext navigationContext) => false;

        public virtual void OnNavigatedFrom(INavigationContext navigationContext) { }

        public virtual async void OnNavigatedTo(INavigationContext navigationContext) => await RefreshAsync();

        public virtual async Task RefreshAsync() => await Task.CompletedTask;

        public override async Task SetBusyAsync(Func<Task> func, string loadingMessage = null)
        {
            IsBusy = true;
            try
            {
                applayer.Show(loadingMessage);
                await func();
            }
            finally
            {
                IsBusy = false;
                applayer.Hide();
            }
        }
    }
}