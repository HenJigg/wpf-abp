using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services.Permission;
using AppFramework.Localization;
using AppFramework.Localization.Dto;
using Prism.Regions;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class LanguageViewModel : NavigationCurdViewModel
    {
        private readonly ILanguageAppService appService;
        private readonly IRegionManager regionManager;

        public LanguageListModel SelectedItem => Map<LanguageListModel>(dataPager.SelectedItem);

        public LanguageViewModel(ILanguageAppService languageAppService, IRegionManager regionManager)
        {
            this.appService = languageAppService;
            this.regionManager = regionManager;
        }

        /// <summary>
        /// 更改文本
        /// </summary>
        private void ChangeTexts()
        {
            NavigationParameters param = new NavigationParameters();
            param.Add("Name", SelectedItem.Name);

            regionManager
                .Regions[AppRegionManager.Main]
                .RequestNavigate(AppViewManager.LanguageText, param);
        }

        /// <summary>
        /// 设置默认语言
        /// </summary>
        private async void SetAsDefaultLanguage()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() =>
                appService.SetDefaultLanguage(new Localization.Dto.SetDefaultLanguageInput()
                {
                    Name = SelectedItem.Name
                }));
            });
        }

        /// <summary>
        /// 删除语言
        /// </summary>
        private async void Delete()
        {
            if (await dialog.Question(Local.Localize("LanguageDeleteWarningMessage", SelectedItem.DisplayName)))
            {
                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(() => appService.DeleteLanguage(
                        new EntityDto(SelectedItem.Id)),
                        RefreshAsync);
                });
            }
        }

        /// <summary>
        /// 获取语言列表
        /// </summary>
        /// <returns></returns>
        private async Task GetLanguages()
        {
            await WebRequest.Execute(() => appService.GetLanguages(),
                      async result =>
                      {
                          dataPager.SetList(new PagedResultDto<ApplicationLanguageListDto>()
                          {
                              Items = result.Items
                          });
                          await Task.CompletedTask;
                      });
        }

        /// <summary>
        /// 刷新语言列表模块
        /// </summary>
        /// <returns></returns>
        public override async Task RefreshAsync()
        {
            await SetBusyAsync(GetLanguages); 
        }

        public override PermissionItem[] GetDefaultPermissionItems()
        {
            return new PermissionItem[]
            {
                new PermissionItem(Permkeys.LanguageEdit, Local.Localize("Change"),()=>Edit()),
                new PermissionItem(Permkeys.LanguageChangeTexts, Local.Localize("ChangeTexts"),()=>ChangeTexts()),
                new PermissionItem(Permkeys.Languages, Local.Localize("SetAsDefaultLanguage"),()=>SetAsDefaultLanguage()),
                new PermissionItem(Permkeys.LanguageDelete, Local.Localize("Delete"),()=>Delete())
            };
        }

    }
}