using Abp.Application.Services.Dto;
using AppFramework.Organizations;
using AppFramework.Organizations.Dto;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Linq;
using System.Threading.Tasks;
using AppFramework.Shared.Services;
using AppFramework.Shared;
using AppFramework.Admin.Models;

namespace AppFramework.Admin.ViewModels
{
    public class AddRolesViewModel : HostDialogViewModel
    {
        #region 字段/属性

        public string Filter
        {
            get { return input.Filter; }
            set { input.Filter = value; OnPropertyChanged(); }
        }

        public IDataPagerService dataPager { get; private set; }
        private readonly IOrganizationUnitAppService appService;

        public FindOrganizationUnitRolesInput input;

        public DelegateCommand QueryCommand { get; private set; }

        #endregion

        public AddRolesViewModel(IOrganizationUnitAppService appService,
            IDataPagerService dataPager)
        {
            input = new FindOrganizationUnitRolesInput();
            QueryCommand = new DelegateCommand(Query);
            this.appService = appService;
            this.dataPager = dataPager;

            dataPager.OnPageIndexChangedEventhandler += RoleOnPageIndexChangedEventhandler;
        }

        private async void RoleOnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {
            input.SkipCount = e.SkipCount;
            input.MaxResultCount = e.PageSize;

            await SetBusyAsync(async () =>
            {
                await FindRoles(input);
            });
        }

        public override async Task Save()
        {
            var roleIds = dataPager.GridModelList
               .Where(t => t is ChooseItem q && q.IsSelected)
               .Select(t => Convert.ToInt32(((ChooseItem)t).Value.Value)).ToArray();

            await SetBusyAsync(async () =>
            {
                await appService.AddRolesToOrganizationUnit(new RolesToOrganizationUnitInput() { OrganizationUnitId = input.OrganizationUnitId, RoleIds = roleIds }).WebAsync(base.Save);
            });
        }

        private async Task FindRoles(FindOrganizationUnitRolesInput input)
        {
            await appService.FindRoles(input).WebAsync(async result => { await dataPager.SetList(new PagedResultDto<ChooseItem>() { Items = result.Items.Select(t => new ChooseItem(t)).ToList() }); });
        }

        private void Query()
        {
            dataPager.PageIndex = 0;
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                input.OrganizationUnitId = parameters.GetValue<long>("Id");
                var pagedResult = parameters.GetValue<PagedResultDto<NameValueDto>>("Value");
                dataPager.SetList(new PagedResultDto<ChooseItem>()
                {
                    Items = pagedResult.Items.Select(t => new ChooseItem(t)).ToList()
                });
            }
        }
    }
}