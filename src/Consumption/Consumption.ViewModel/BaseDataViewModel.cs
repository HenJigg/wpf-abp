/*
*
* 文件名    ：BaseDataViewModel                             
* 程序说明  : 数据视图基类-GUID
* 更新时间  : 2020-07-11 17：53
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
* 
* 更新日期: 2020-09-11
* 更新内容: 重构基类的实现
*/


namespace Consumption.ViewModel
{
    using Consumption.Core.Response;
    using Consumption.Core.Enums;
    using Consumption.Core.Interfaces;
    using Consumption.ViewModel.Common;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;
    using System.Threading.Tasks;
    using Consumption.Common.Contract;
    using GalaSoft.MvvmLight.Messaging;
    using Consumption.Core.Common;
    using Consumption.Core.Collections;
    using Consumption.Core.Query;
    using Consumption.ViewModel.Interfaces;
    using Consumption.Core.Entity;
    using System.Linq;
    using Consumption.Core.Aop;

    /// <summary>
    /// 通用基类(实现CRUD/数据分页..)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseRepository<TEntity> : ViewModelBase where TEntity : BaseEntity
    {
        public readonly IConsumptionRepository<TEntity> repository;

        public BaseRepository() { }

        public BaseRepository(IConsumptionRepository<TEntity> repository)
        {
            this.repository = repository;
            QueryCommand = new RelayCommand(Query);
            ExecuteCommand = new RelayCommand<string>(arg => Execute(arg));
        }

        #region ICrud (增删改查接口~喵)

        private int selectPageIndex;
        private string search;
        private TEntity gridModel;
        private ObservableCollection<TEntity> gridModelList;
        public TEntity GridModel
        {
            get { return gridModel; }
            set { gridModel = value; RaisePropertyChanged(); }
        }
        public int SelectPageIndex
        {
            get { return selectPageIndex; }
            set { selectPageIndex = value; RaisePropertyChanged(); }
        }
        public string Search
        {
            get { return search; }
            set { search = value; RaisePropertyChanged(); }
        }
        public ObservableCollection<TEntity> GridModelList
        {
            get { return gridModelList; }
            set { gridModelList = value; RaisePropertyChanged(); }
        }
        public RelayCommand QueryCommand { get; }
        public RelayCommand<string> ExecuteCommand { get; }

        /// <summary>
        /// 查询
        /// </summary>
        public virtual async void Query()
        {
            await GetPageData(this.PageIndex);
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="arg"></param>
        [GlobalLoger]
        public virtual void Execute(string arg)
        {
            /*
             * 这里使用string来做弱类型处理,防止使用枚举,
             * 其他页面需要重新该方法
             */
            switch (arg)
            {
                case "添加": AddAsync(); break;
                case "修改": UpdateAsync(); break;
                case "删除": DeleteAsync(); break;
                case "保存": SaveAsync(); break;
                case "取消": Cancel(); break;
            }
        }

        public virtual void AddAsync()
        {
            this.CreateDeaultCommand();
            SelectPageIndex = 1;
        }

        public virtual void Cancel()
        {
            InitPermissions(this.AuthValue);
            SelectPageIndex = 0;
        }

        public virtual async void DeleteAsync()
        {
            if (GridModel != null)
            {
                if (await Msg.Question("Confirm that the current selection is deleted??"))
                {
                    var r = await repository.DeleteAsync(GridModel.Id);
                    if (r != null && r.success)
                        await GetPageData(0);
                    else
                        Messenger.Default.Send("Delete data exception.!", "Snackbar");
                }
            }
        }

        [GlobalProgress]
        public virtual void SaveAsync()
        {
            //Before you save, you need to verify the validity of the data.
            if (GridModel != null)
            {
                repository.SaveAsync(GridModel);
                InitPermissions(this.AuthValue);
                SelectPageIndex = 0;
            }
        }

        [GlobalProgress]
        public virtual async void UpdateAsync()
        {
            if (GridModel != null)
            {
                var baseResponse = await repository.GetAsync(GridModel.Id);
                if (baseResponse != null && baseResponse.success)
                {
                    GridModel = baseResponse.dynamicObj;
                    this.CreateDeaultCommand();
                    SelectPageIndex = 1;
                }
                else
                    Messenger.Default.Send("Get data exception!", "Snackbar");
            }
        }

        #endregion

        #region IDataPager (数据分页~喵)
        public RelayCommand GoHomePageCommand { get { return new RelayCommand(() => GoHomePage()); } }
        public RelayCommand GoOnPageCommand { get { return new RelayCommand(() => GoOnPage()); } }
        public RelayCommand GoNextPageCommand { get { return new RelayCommand(() => GoNextPage()); } }
        public RelayCommand GoEndPageCommand { get { return new RelayCommand(() => GoEndPage()); } }

        private int totalCount = 0;
        private int pageSize = 30;
        private int pageIndex = 0;
        private int pageCount = 0;

        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get { return totalCount; } set { totalCount = value; RaisePropertyChanged(); } }

        /// <summary>
        /// 当前页大小
        /// </summary>
        public int PageSize { get { return pageSize; } set { pageSize = value; RaisePropertyChanged(); } }

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get { return pageIndex; } set { pageIndex = value; RaisePropertyChanged(); } }

        /// <summary>
        /// 分页总数
        /// </summary>
        public int PageCount { get { return pageCount; } set { pageCount = value; RaisePropertyChanged(); } }

        /// <summary>
        /// 首页
        /// </summary>
        public virtual async void GoHomePage()
        {
            if (this.PageIndex == 0) return;
            PageIndex = 0;
            await GetPageData(PageIndex);
        }

        /// <summary>
        /// 上一页
        /// </summary>
        public virtual async void GoOnPage()
        {
            if (this.PageIndex == 0) return;
            PageIndex--;
            await this.GetPageData(PageIndex);
        }

        /// <summary>
        /// 下一页
        /// </summary>
        public virtual async void GoNextPage()
        {
            if (this.PageIndex == PageCount) return;
            PageIndex++;
            await this.GetPageData(PageIndex);
        }

        /// <summary>
        /// 尾页
        /// </summary>
        public virtual async void GoEndPage()
        {
            this.PageIndex = PageCount;
            await GetPageData(PageCount);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="pageIndex"></param>
        public virtual async Task GetPageData(int pageIndex)
        {
            var r = await repository.GetAllListAsync(new QueryParameters()
            {
                PageIndex = this.PageIndex,
                PageSize = this.PageSize,
                Search = this.Search
            });
            if (r != null && r.success)
            {
                GridModelList = new ObservableCollection<TEntity>();
                r.dynamicObj.Items?.ToList().ForEach(arg =>
                {
                    GridModelList.Add(arg);
                });
                TotalCount = r.dynamicObj.Items.Count;
                SetPageCount();
            }
        }

        /// <summary>
        /// 设置页数
        /// </summary>
        public virtual void SetPageCount()
        {
            PageCount = Convert.ToInt32(Math.Ceiling((double)TotalCount / (double)PageSize));
        }
        #endregion

        #region IAuthority (权限内容~)

        /// <summary>
        /// 创建页面默认命令
        /// </summary>
        private void CreateDeaultCommand()
        {
            ToolBarCommandList.Clear();
            ToolBarCommandList.Add(new ButtonCommand() { CommandName = "保存", CommandColor = "#0066FF", CommandKind = "ContentSave" });
            ToolBarCommandList.Add(new ButtonCommand() { CommandName = "取消", CommandColor = "#FF6633", CommandKind = "Cancel" });
        }

        private ObservableCollection<ButtonCommand> toolBarCommandList;
        public ObservableCollection<ButtonCommand> ToolBarCommandList
        {
            get { return toolBarCommandList; }
            set { toolBarCommandList = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 页面权限值
        /// </summary>
        public int AuthValue { get; private set; }

        /// <summary>
        /// 初始化权限
        /// </summary>
        public void InitPermissions(int AuthValue)
        {
            this.AuthValue = AuthValue;
            ToolBarCommandList = new ObservableCollection<ButtonCommand>();
            Contract.AuthItems.ForEach(arg =>
            {
                if ((AuthValue & arg.AuthValue) == arg.AuthValue)
                    ToolBarCommandList.Add(new ButtonCommand()
                    {
                        CommandName = arg.AuthName,
                        CommandKind = arg.AuthKind,
                        CommandColor = arg.AuthColor
                    });
            });
        }
        #endregion
    }
}
