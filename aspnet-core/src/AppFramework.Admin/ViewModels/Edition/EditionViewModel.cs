using Abp.Application.Services.Dto;
using AppFramework.Editions;
using AppFramework.Editions.Dto;
using AppFramework.Shared;
using AppFramework.Shared.Services.Permission;
using AppFramework.Admin.ViewModels.Shared;
using Prism.Regions;
using System.Threading.Tasks;

namespace AppFramework.Admin.ViewModels
{
    public class EditionViewModel : NavigationCurdViewModel
    {
        private readonly IEditionAppService appService;

        public EditionViewModel(IEditionAppService appService)
        {
            Title = Local.Localize("Editions");
            this.appService = appService;
        }

        /// <summary>
        /// 删除版本信息
        /// </summary>
        private async void Delete()
        {
            if (dataPager.SelectedItem is EditionListDto item)
            {
                if (await dialog.Question(Local.Localize("EditionDeleteWarningMessage", item.DisplayName)))
                {
                    await SetBusyAsync(async () =>
                    {
                        await appService.DeleteEdition(new EntityDto(item.Id)).WebAsync(async () => await OnNavigatedToAsync());
                    });
                }
            }
        }

        /// <summary>
        /// 获取版本列表
        /// </summary>
        /// <returns></returns>
        private async Task GetEditions()
        {
            await SetBusyAsync(async () =>
            {
                await appService.GetEditions().WebAsync(dataPager.SetList);
            });
        }

        /// <summary>
        /// 刷新版本模块
        /// </summary>
        /// <returns></returns>
        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            await GetEditions();
        }

        public override PermissionItem[] CreatePermissionItems()
        {
            return new PermissionItem[]
            {
                new PermissionItem(AppPermissions.EditionEdit, Local.Localize("EditEdition"),Edit),
                new PermissionItem(AppPermissions.EditionDelete, Local.Localize("Delete"),Delete),
            };
        }
    }
}
