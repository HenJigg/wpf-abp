using AppFramework.Shared;
using AppFramework.Shared.Models.Configuration;
using AppFramework.Configuration.Host;
using AppFramework.Configuration.Host.Dto;
using AppFramework.Editions.Dto;
using AppFramework.Admin.ViewModels.Shared;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AppFramework.Common;
using CommunityToolkit.Mvvm.Input;

namespace AppFramework.Admin.ViewModels
{
    public partial class SettingsViewModel : NavigationViewModel
    {
        #region 字段/属性

        private readonly IHostSettingsAppService appService;
        private readonly ICommonLookupAppService lookupAppService;
        private HostSettingsEditModel setting;
        private SubscribableEditionComboboxItemDto selectedEdition;
        private ObservableCollection<SubscribableEditionComboboxItemDto> editions;

        public HostSettingsEditModel Setting
        {
            get { return setting; }
            set { setting = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 选中版本
        /// </summary>
        public SubscribableEditionComboboxItemDto SelectedEdition
        {
            get { return selectedEdition; }
            set
            {
                selectedEdition = value;
                if (selectedEdition != null)
                    setting.TenantManagement.DefaultEditionId = Convert.ToInt32(value.Value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 版本列表
        /// </summary>
        public ObservableCollection<SubscribableEditionComboboxItemDto> Editions
        {
            get => editions;
            set
            {
                editions = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public SettingsViewModel(IHostSettingsAppService appService,
            ICommonLookupAppService lookupAppService)
        {
            Title = Local.Localize("Settings");
            this.appService = appService;
            this.lookupAppService = lookupAppService;

            editions = new ObservableCollection<SubscribableEditionComboboxItemDto>();
        }

        [RelayCommand]
        private async void Save()
        {
            //验证输入合法性...

            var input = Map<HostSettingsEditDto>(Setting);
            await SetBusyAsync(async () =>
             {
                 await appService.UpdateAllSettings(input).WebAsync();
             });
        }

        /// <summary>
        /// 获取系统设置信息
        /// </summary>
        /// <returns></returns>
        private async Task GetSettings()
        {
            await appService.GetAllSettings().WebAsync(async result =>
              {
                  Setting = Map<HostSettingsEditModel>(result);
                  await Task.CompletedTask;
              });
        }

        /// <summary>
        /// 获取版本列表
        /// </summary>
        /// <returns></returns>
        private async Task GetEditions()
        {
            await lookupAppService.GetEditionsForCombobox().WebAsync(async result =>
              {
                  foreach (var item in result.Items)
                      Editions.Add(item);

                  await Task.CompletedTask;
              });
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext)
        {
            await SetBusyAsync(async () =>
            {
                await GetSettings();
                await GetEditions();
            });
        }
    }
}
