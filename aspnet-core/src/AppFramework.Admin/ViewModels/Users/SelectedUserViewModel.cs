using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Dto;
using AppFramework.Shared;
using AppFramework.Shared.Services;
using CommunityToolkit.Mvvm.Input;
using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;

namespace AppFramework.Admin.ViewModels
{
    public partial class SelectedUserViewModel : HostDialogViewModel
    {
        private readonly ICommonLookupAppService appService;
        public IDataPagerService dataPager { get; private set; }
        private FindUsersInput input;

        public SelectedUserViewModel(IDataPagerService dataPager,
            ICommonLookupAppService appService)
        {
            this.dataPager = dataPager;
            this.appService = appService;
            input = new FindUsersInput()
            {
                MaxResultCount = 10,
            };
            this.dataPager.OnPageIndexChangedEventhandler += DataPager_OnPageIndexChangedEventhandler;
        }

        [RelayCommand]
        private void SelectedUser(NameValueDto obj)
        {
            Save(Convert.ToInt32(obj.Value));
        }

        private async void DataPager_OnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {
            input.SkipCount = e.SkipCount;
            input.MaxResultCount = e.PageSize;

            await GetFindUsers(input);
        }

        private async Task GetFindUsers(FindUsersInput input)
        {
            await SetBusyAsync(async () =>
            {
                await appService.FindUsers(input).WebAsync(dataPager.SetList);
            });
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await GetFindUsers(input);
        }
    }
}
