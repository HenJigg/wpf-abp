using Abp.Application.Services.Dto;
using AppFramework.Shared;
using AppFramework.Admin.Models;
using AppFramework.Shared.Services.Permission;
using AppFramework.Version;
using AppFramework.Version.Dtos;
using Prism.Regions;
using System.Threading.Tasks;
using AppFramework.Shared.Services;

namespace AppFramework.Admin.ViewModels
{
    public class VersionManagerViewModel : NavigationCurdViewModel
    {
        private readonly IAbpVersionsAppService appService;
        public GetAllAbpVersionsInput input;
        public VersionListModel SelectedItem => Map<VersionListModel>(dataPager.SelectedItem);

        public VersionManagerViewModel(IAbpVersionsAppService appService)
        {
            Title = Local.Localize("VersionManager");
            this.appService = appService;
            input = new GetAllAbpVersionsInput()
            {
                MaxResultCount = 10
            };
            dataPager.OnPageIndexChangedEventhandler += DataPager_OnPageIndexChangedEventhandler;
        }

        private void DataPager_OnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        { }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            await SetBusyAsync(async () =>
            {
                await GetVersions(input);
            });
        }

        public override PermissionItem[] CreatePermissionItems()
        {
            return new PermissionItem[]
            {
                new PermissionItem(AppPermissions.VersionsEdit,Local.Localize("Change"),Edit),
                new PermissionItem(AppPermissions.VersionsDelete, Local.Localize("Delete"),Delete),
            };
        }

        private async Task GetVersions(GetAllAbpVersionsInput filter)
        {
            await appService.GetAll(filter).WebAsync(dataPager.SetList);
        }

        private async void Delete()
        {
            var result = await dialog.Question(Local.Localize("VersionDeleteWarningMessage", SelectedItem.Name));
            if (result)
            {
                await SetBusyAsync(async () =>
                {
                    await appService.Delete(new EntityDto(SelectedItem.Id)).WebAsync(async () => await OnNavigatedToAsync());
                });
            }
        }
    }
}
