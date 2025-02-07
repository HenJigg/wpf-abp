namespace AppFramework.Shared.ViewModels
{
    using Abp.Application.Services.Dto;
    using AppFramework.Organizations;
    using AppFramework.Organizations.Dto;
    using Prism.Commands;
    using Prism.Navigation;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using AppFramework.Shared.Models;
    using AppFramework.Shared;
    using Abp.Organizations;

    public class OrganizationViewModel : NavigationMasterViewModel
    {
        private readonly IOrganizationUnitAppService appService;

        public DelegateCommand<OrganizationListModel> AddRoleCommand { get; private set; }
        public DelegateCommand<OrganizationListModel> AddUserCommand { get; private set; }
        public DelegateCommand<OrganizationListModel> AddSubUnitCommand { get; private set; }

        public DelegateCommand SaveCommand { get; private set; }

        public OrganizationViewModel(IOrganizationUnitAppService appService)
        {
            this.appService = appService;

            SaveCommand=new DelegateCommand(Save);
            AddSubUnitCommand = new DelegateCommand<OrganizationListModel>(AddSubUnit);
        }

        #region 新增组织/子组织

        private long? ParentId;

        private string organizationName;

        public string OrganizationName
        {
            get { return organizationName; }
            set { organizationName = value; RaisePropertyChanged(); }
        }

        public override void Add()
        {
            OrganizationName=string.Empty;
            messenger.Send("OrganizationViewIsVisible", true);
        }

        private async void Save()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() =>
                      appService.CreateOrganizationUnit(new CreateOrganizationUnitInput()
                      {
                          DisplayName = OrganizationName
                      }), RefreshAsync);
            });
            messenger.Send("OrganizationViewIsVisible", false);
        }

        #endregion

        private async void AddSubUnit(OrganizationListModel obj)
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("Id", obj.Id);

            await navigationService.NavigateAsync(GetPageName("Details"), param);
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.GetOrganizationUnits(), RefreshSuccessed);
            });
        }

        protected virtual async Task RefreshSuccessed(ListResultDto<OrganizationUnitDto> pagedResult)
        {
            var organizationUnits = pagedResult.Items?.Select(t => new OrganizationListModel
            {
                Id = t.Id,
                Code = t.Code,
                ParentId = t.ParentId,
                RoleCount = t.RoleCount,
                DisplayName = t.DisplayName,
                MemberCount = t.MemberCount,
            }).ToList();

            dataPager.GridModelList = BuildOrganizationTree(organizationUnits);

            await Task.CompletedTask;
        }

        public ObservableCollection<object> BuildOrganizationTree(
           List<OrganizationListModel> organizationUnits, long? parentId = null)
        {
            var masters = organizationUnits
                .Where(x => x.ParentId == parentId).ToList();

            var childs = organizationUnits
                .Where(x => x.ParentId != parentId).ToList();

            foreach (OrganizationListModel dpt in masters)
                dpt.Items = BuildOrganizationTree(childs, dpt.Id);

            return new ObservableCollection<object>(masters);
        }
    }
}