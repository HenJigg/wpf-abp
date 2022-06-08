using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Organizations;
using AppFramework.Organizations.Dto;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class AddUsersViewModel : HostDialogViewModel
    {
        #region 字段/属性

        public string Filter
        {
            get { return input.Filter; }
            set
            {
                input.Filter = value;
                RaisePropertyChanged();
            }
        }

        public IDataPagerService dataPager { get; private set; }
        private readonly IOrganizationUnitAppService appService;

        public DelegateCommand QueryCommand { get; private set; }
        public FindOrganizationUnitUsersInput input;

        #endregion

        public AddUsersViewModel(IOrganizationUnitAppService appService,
            IDataPagerService dataPager)
        {
            input = new FindOrganizationUnitUsersInput();
            QueryCommand = new DelegateCommand(Query);
            this.dataPager = dataPager;
            this.appService = appService;

            dataPager.OnPageIndexChangedEventhandler += UserOnPageIndexChangedEventhandler;
        }

        private async void UserOnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {
            input.SkipCount = e.SkipCount;
            input.MaxResultCount = e.PageSize;

            await SetBusyAsync(async () =>
             {
                 await FindUsers(input);
             });
        }

        protected override async void Save()
        {
            var userIds = dataPager.GridModelList
                .Where(t => t is ChooseItem q && q.IsSelected)
                .Select(t => Convert.ToInt64(((ChooseItem)t).Value.Value)).ToArray();

            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.AddUsersToOrganizationUnit(
                    new UsersToOrganizationUnitInput()
                    {
                        OrganizationUnitId = input.OrganizationUnitId,
                        UserIds = userIds
                    }), () =>
                    {
                        base.Save();
                        return Task.CompletedTask;
                    });
            });
        }

        private async Task FindUsers(FindOrganizationUnitUsersInput input)
        {
            await WebRequest.Execute(() => appService.FindUsers(input),
                async result =>
                {
                    dataPager.SetList(new PagedResultDto<ChooseItem>()
                    {
                        Items = result.Items.Select(t => new ChooseItem(t)).ToList()
                    });
                    await Task.CompletedTask;
                });
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