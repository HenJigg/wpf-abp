using Abp.Application.Services.Dto;
using AppFramework.Shared;
using AppFramework.Shared.Services.Permission;
using AppFramework.Editions;
using AppFramework.Admin.Models;
using AppFramework.MultiTenancy;
using AppFramework.MultiTenancy.Dto;
using Prism.Commands;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Prism.Services.Dialogs;
using AppFramework.Admin.ViewModels.Shared;
using Prism.Regions;
using AppFramework.Shared.Services;
using CommunityToolkit.Mvvm.Input;

namespace AppFramework.Admin.ViewModels
{
    public partial class TenantViewModel : NavigationCurdViewModel
    {
        #region 字段/属性

        private readonly ITenantAppService appService;
        private readonly IEditionAppService editionAppService;

        private bool isSubscription;
        private bool isCreation;
        private GetTenantsFilter filter;
        private EditionListModel edition;
        private ObservableCollection<EditionListModel> editions;

        /// <summary>
        /// 启用订阅日期查询
        /// </summary>
        public bool IsSubscription
        {
            get { return isSubscription; }
            set
            {
                isSubscription = value;
                if (value)
                {
                    Filter.SubscriptionEndDateStart = null;
                    Filter.SubscriptionEndDateEnd = null;
                }
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 启用创建时间查询
        /// </summary>
        public bool IsCreation
        {
            get { return isCreation; }
            set
            {
                isCreation = value;
                if (value)
                {
                    Filter.CreationDateStart = null;
                    Filter.CreationDateEnd = null;
                }
                OnPropertyChanged();
            }
        }

        public GetTenantsFilter Filter
        {
            get { return filter; }
            set { filter = value; OnPropertyChanged(); }
        }

        public EditionListModel Edition
        {
            get { return edition; }
            set { edition = value; OnPropertyChanged(); }
        }

        public ObservableCollection<EditionListModel> Editions
        {
            get { return editions; }
            set { editions = value; OnPropertyChanged(); }
        }

        #endregion

        public TenantListModel SelectedItem => Map<TenantListModel>(dataPager.SelectedItem);

        public TenantViewModel(ITenantAppService appService, IEditionAppService editionAppService)
        {
            Title = Local.Localize("TenantManagement");
            filter = new GetTenantsFilter()
            {
                EditionIdSpecified = false,
                MaxResultCount = 10,
                SkipCount = 0,
            };

            this.appService = appService;
            this.editionAppService = editionAppService;
            dataPager.OnPageIndexChangedEventhandler += TenantOnPageIndexChangedEventhandler;
            editions = new ObservableCollection<EditionListModel>();
        }

        private async void TenantOnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {
            filter.SkipCount = e.SkipCount;
            filter.MaxResultCount = e.PageSize;

            await SetBusyAsync(GetTenants);
        }

        /// <summary>
        /// 搜索
        /// </summary>
        [RelayCommand]
        private void Search()
        {
            dataPager.PageIndex = 0;
        }

        /// <summary>
        /// 获取租户列表
        /// </summary>
        /// <returns></returns>
        private async Task GetTenants()
        {
            var input = Map<GetTenantsInput>(filter);
            await appService.GetTenants(input).WebAsync(dataPager.SetList);
        }

        /// <summary>
        /// 获取筛选版本列表
        /// </summary>
        /// <returns></returns>
        public async Task GetAllEditions()
        {
            if (Editions.Count > 0) return;

            await editionAppService.GetEditions().WebAsync(async result =>
             {
                 Editions.Clear();
                 foreach (var item in Map<List<EditionListModel>>(result.Items))
                     Editions.Add(item);

                 await Task.CompletedTask;
             });
        }

        #region 修改租户/使用当前租户登录/解锁/删除

        /// <summary>
        /// 修改租户功能
        /// </summary>
        /// <param name="Id"></param>
        private async void TenantChangeFeatures()
        {
            GetTenantFeaturesEditOutput output = null;
            await SetBusyAsync(async () =>
            {
                await appService.GetTenantFeaturesForEdit(new EntityDto(SelectedItem.Id)).WebAsync(async result =>
                {
                    output = result;
                    await Task.CompletedTask;
                });
            });

            if (output == null) return;

            DialogParameters param = new DialogParameters();
            param.Add("Id", SelectedItem.Id);
            param.Add("Value", output);

            await dialog.ShowDialogAsync(AppViews.TenantChangeFeatures, param);
        }

        /// <summary>
        /// 使用租户登录
        /// </summary>
        private void TenantImpersonation()
        {
            //..使用当前租户登录
        }

        /// <summary>
        /// 解锁租户
        /// </summary>
        private async void Unlock()
        {
            await SetBusyAsync(async () =>
            {
                await appService.UnlockTenantAdmin(new EntityDto(SelectedItem.Id))
                                .WebAsync(async () => await OnNavigatedToAsync());
            });
        }

        /// <summary>
        /// 删除租户
        /// </summary>
        private async void Delete()
        {
            var result = await dialog.Question(Local.Localize("TenantDeleteWarningMessage", SelectedItem.TenancyName));
            if (result)
            {
                await SetBusyAsync(async () =>
                {
                    await appService.DeleteTenant(new EntityDto(SelectedItem.Id))
                                    .WebAsync(async () => await OnNavigatedToAsync());
                });
            }
        }

        #endregion

        /// <summary>
        /// 刷新租户模块
        /// </summary>
        /// <returns></returns>
        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            await SetBusyAsync(async () =>
            {
                await GetAllEditions();
                await GetTenants();
            });
        }

        public override PermissionItem[] CreatePermissionItems()
        {
            return new PermissionItem[]
             {
                new PermissionItem(AppPermissions.TenantImpersonation, Local.Localize("LoginAsThisTenant"),TenantImpersonation),
                new PermissionItem(AppPermissions.TenantEdit, Local.Localize("Change"),Edit),
                new PermissionItem(AppPermissions.TenantChangeFeatures, Local.Localize("Features"),TenantChangeFeatures),
                new PermissionItem(AppPermissions.TenantDelete, Local.Localize("Delete"),Delete),
                new PermissionItem(AppPermissions.TenantUnlock, Local.Localize("Unlock"),Unlock)
             };
        }
    }
}
