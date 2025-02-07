using AppFramework.Auditing;
using AppFramework.Auditing.Dto;
using AppFramework.Shared; 
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Regions;
using AppFramework.Shared.Services;
using AppFramework.Admin.Models;
using CommunityToolkit.Mvvm.Input;

namespace AppFramework.Admin.ViewModels
{
    public partial class AuditLogsViewModel : NavigationCurdViewModel
    {
        public AuditLogsViewModel(IAuditLogAppService appService)
        {
            Title = Local.Localize("AuditLogs");
            IsAdvancedFilter = false;
            filter = new GetAuditLogsFilter()
            {    
                StartDate = DateTime.Now.AddDays(-30),
                EndDate = DateTime.Now,
                //MaxResultCount = AppConsts.DefaultPageSize
            };
            entityChangeFilter = new GetEntityChangeFilter()
            {
                StartDate = DateTime.Now.AddDays(-30),
                EndDate = DateTime.Now,
                MaxResultCount = AppConsts.DefaultPageSize
            };
            this.appService = appService;
             
            logsdataPager = ContainerLocator.Container.Resolve<IDataPagerService>();

            //绑定分页组件索引改变事件
            logsdataPager.OnPageIndexChangedEventhandler += EntityChangesOnPageIndexChangedEventhandler;
            dataPager.OnPageIndexChangedEventhandler += AuditLogsOnPageIndexChangedEventhandler;
        }

        #region 字段/属性

        public IDataPagerService logsdataPager { get; private set; }

        private readonly IAuditLogAppService appService;
        private string filterTitle = string.Empty;
        private bool isAdvancedFilter;
        private GetAuditLogsFilter filter;
        private GetEntityChangeFilter entityChangeFilter;

        private int selectedIndex;

        /// <summary>
        /// 错误状态选项: 全部/错误
        /// </summary>
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;

                if (selectedIndex == 0)
                    filter.HasException = null;
                else if (selectedIndex == 1)
                    filter.HasException = false;
                else
                    filter.HasException = true;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 当前筛选标题 [展开/收缩]
        /// </summary>
        public string FilerTitle
        {
            get { return filterTitle; }
            set { filterTitle = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 高级筛选
        /// </summary>
        public bool IsAdvancedFilter
        {
            get { return isAdvancedFilter; }
            set
            {
                isAdvancedFilter = value;

                FilerTitle = value ? "△ " + Local.Localize("HideAdvancedFilters") : "▽ " + Local.Localize("ShowAdvancedFilters");
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 审计日志筛选条件
        /// </summary>
        public GetAuditLogsFilter Filter
        {
            get { return filter; }
            set { filter = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 更改日志筛选条件
        /// </summary>
        public GetEntityChangeFilter EntityChangeFilter
        {
            get { return entityChangeFilter; }
            set { entityChangeFilter = value; OnPropertyChanged(); }
        }
          
        #endregion

        #region 审计日期

        [RelayCommand]
        private void Advanced()
        {
            IsAdvancedFilter = !IsAdvancedFilter;
        }

        /// <summary>
        /// 查看操作日志详情
        /// </summary>
        [RelayCommand]
        private void ViewLog()
        {
            DialogParameters param = new DialogParameters();
            param.Add("Value", dataPager.SelectedItem);
            dialog.ShowDialogAsync(AppViews.AuditLogDetails, param);
        }

        /// <summary>
        /// 搜索操作日志
        /// </summary>
        [RelayCommand]
        private void Search()
        {
            /*
             * 搜索结果的时候,只需要重置页索引为0即可,因为页索引变化会触发搜索事件
             * 相关搜索条件则通过绑定的方式自动更新到筛选条件上. 
             * 
             * 说明: 这里一般验证筛选条件合法性即可.
             */
            dataPager.PageIndex = 0;
        }

        /// <summary>
        /// 获取审计日期数据
        /// </summary>
        /// <returns></returns>
        private async Task GetAuditLogs(GetAuditLogsFilter input)
        {
            input.StartDate=input.StartDate.GetFirstDate();
            input.EndDate=input.EndDate.GetLastDate();
            
            await SetBusyAsync(async () =>
            {
                await appService.GetAuditLogs(Map<GetAuditLogsInput>(input)).WebAsync(dataPager.SetList); 
            });
        }

        private async void AuditLogsOnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {
            filter.SkipCount = e.SkipCount;
            filter.MaxResultCount = e.PageSize;
            await GetAuditLogs(filter);
        }

        #endregion

        #region 更改日志

        /// <summary>
        /// 搜索更改日志
        /// </summary>
        [RelayCommand]
        private void SearchChanged()
        {
            dataPager.PageIndex = 0;
        }

        /// <summary>
        /// 获取更改日志
        /// </summary>
        /// <returns></returns>
        private async Task GetEntityChanges(GetEntityChangeFilter input)
        {
            await appService.GetEntityChanges(Map<GetEntityChangeInput>(input)).WebAsync(logsdataPager.SetList);
        }

        /// <summary>
        /// 查看更改日志详情
        /// </summary>
        [RelayCommand]
        private void ViewChangedLog()
        { }

        private async void EntityChangesOnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {
            entityChangeFilter.SkipCount = e.SkipCount;
            entityChangeFilter.MaxResultCount = e.PageSize;
            await GetEntityChanges(entityChangeFilter);
        }

        #endregion

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <returns></returns>
        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            await SetBusyAsync(async () =>
            {
                await GetAuditLogs(filter);
                await GetEntityChanges(entityChangeFilter);
            });
        }
    }
}