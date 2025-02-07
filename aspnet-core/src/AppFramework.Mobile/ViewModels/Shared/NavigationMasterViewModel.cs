namespace AppFramework.Shared.ViewModels
{
    using Prism.Commands;
    using Prism.Navigation;
    using Prism.Ioc;
    using AppFramework.Shared.Services.Datapager;
    using AppFramework.Shared.Core;
    using Prism.Regions.Navigation;
    using Syncfusion.ListView.XForms;

    public class NavigationMasterViewModel : RegionViewModel
    {
        public NavigationMasterViewModel()
        {
            AddCommand = new DelegateCommand(Add);
            EditCommand=new DelegateCommand<object>(Edit);
            LoadMoreCommand = new DelegateCommand(LoadMore);
            RefreshCommand = new DelegateCommand(async () => await RefreshAsync());

            dataPager = ContainerLocator.Container.Resolve<IDataPagerService>();

            messenger = ContainerLocator.Container.Resolve<IMessenger>();
            messenger.Sub(this.GetType().Name, async () => await RefreshAsync());
        }

        public readonly IMessenger messenger;
        public IDataPagerService dataPager { get; set; }
        public DelegateCommand AddCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; private set; }
        public DelegateCommand LoadMoreCommand { get; private set; }

        public DelegateCommand<object> EditCommand { get; private set; }

        public virtual async void Add() => await navigationService.NavigateAsync(GetPageName("Details"));

        public virtual async void Edit(object args)
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("Value", args);

            await navigationService.NavigateAsync(GetPageName("Details"), param);
        }

        public virtual void LoadMore() { }

        protected string GetPageName(string methodName) => this.GetType().Name.Replace("ViewModel", $"{methodName}View");

        public override void OnNavigatedFrom(INavigationContext navigationContext)
        {
            messenger.UnSub(this.GetType().Name);
            base.OnNavigatedFrom(navigationContext);
        }
    }
}