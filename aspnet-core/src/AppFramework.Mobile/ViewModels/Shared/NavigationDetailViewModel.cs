using Prism.Commands;
using Prism.Navigation;
using Prism.Ioc;
using System.Threading.Tasks;
using AppFramework.Shared.Extensions;
using System.Security.Permissions;

namespace AppFramework.Shared.ViewModels
{
    public class NavigationDetailViewModel : ViewModelBase, INavigationAware
    {
        private string pageTitle;
        private bool isNewCreate;
        private bool isDeleteButtonVisible;

        public bool IsNewCeate
        {
            get => isNewCreate;
            set
            {
                isNewCreate = value;
                IsDeleteButtonVisible = !isNewCreate && permissionService.HasPermission(AppPermissions.TenantDelete);
                PageTitle = isNewCreate ? Local.Localize(LocalizationKeys.CreatingNewTenant) : Local.Localize(LocalizationKeys.EditTenant);
                RaisePropertyChanged();
            }
        }

        public bool IsDeleteButtonVisible
        {
            get => isDeleteButtonVisible;
            set
            {
                isDeleteButtonVisible = value;
                RaisePropertyChanged();
            }
        }

        public string PageTitle
        {
            get => pageTitle;
            set
            {
                pageTitle = value;
                RaisePropertyChanged();
            }
        }

        private readonly IPermissionService permissionService;
        public readonly INavigationService navigationService;
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand SaveCommand { get; private set; }

        public DelegateCommand DeleteCommand { get; private set; }

        public NavigationDetailViewModel()
        {
            permissionService= ContainerLocator.Container.Resolve<IPermissionService>();
            navigationService = ContainerLocator.Container.Resolve<INavigationService>();

            SaveCommand = new DelegateCommand(Save);
            DeleteCommand=new DelegateCommand(Delete);
            GoBackCommand = new DelegateCommand(async () => await GoBackAsync());
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters) { }

        public virtual void OnNavigatedTo(INavigationParameters parameters) { }

        public virtual async Task GoBackAsync() => await navigationService.GoBackAsync(null);

        public virtual void Save() { }

        public virtual void Delete() { }
    }
}
