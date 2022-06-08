using Abp.Application.Services.Dto;
using AppFramework.Common; 
using AppFramework.Common.Services.Permission;
using AppFramework.Editions;
using AppFramework.Editions.Dto;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class EditionViewModel : NavigationCurdViewModel
    {
        private readonly IEditionAppService appService;

        public EditionViewModel(IEditionAppService appService)
        {
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
                        await WebRequest.Execute(() =>
                                appService.DeleteEdition(new EntityDto(item.Id)),
                            RefreshAsync);
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
                await WebRequest.Execute(() => appService.GetEditions(),
                        async result =>
                        {
                            dataPager.SetList(new PagedResultDto<EditionListDto>()
                            {
                                Items = result.Items
                            });
                            await Task.CompletedTask;
                        });
            });
        }

        /// <summary>
        /// 刷新版本模块
        /// </summary>
        /// <returns></returns>
        public override async Task RefreshAsync()
        {
            await GetEditions();
        }

        public override PermissionItem[] GetDefaultPermissionItems()
        {
            return new PermissionItem[]
            {
                new PermissionItem(Permkeys.EditionEdit, Local.Localize("EditEdition"),()=>Edit()),
                new PermissionItem(Permkeys.EditionDelete, Local.Localize("Delete"),()=>Delete()),
            };
        } 
    }
}
