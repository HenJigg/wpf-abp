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

    /// <summary>
    /// 基础数据基类
    /// 
    /// 用于:
    ///   1. 实现基础的数据列表展示功能
    ///   2. 实现基础的增删改查单页功能
    ///   3. 实现基础页面的单页数据分页功能
    /// </summary>
    public class BaseDataViewModel<T> :
        ViewModelBase,
        IAuthority,
        IDataPager
        where T : class, new()
    {
        public BaseDataViewModel()
        {
            QueryCommand = new RelayCommand(Query);
            ExecuteCommand = new RelayCommand<string>(arg => Execute(arg));
        }

        #region CURD

        private T gridModel = null;
        private string selectPageTitle;
        private int selectPageIndex = 0;
        private ActionMode Mode { get; set; }
        private string searchText = string.Empty;
        private ObservableCollection<T> gridModelList = null;

        /// <summary>
        /// 当前选择页的标题
        /// </summary>
        public string SelectPageTitle
        {
            get { return selectPageTitle; }
            set { selectPageTitle = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 搜索内容
        /// </summary>
        public string SearchText
        {
            get { return searchText; }
            set { searchText = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 当前选中页
        /// </summary>
        public int SelectPageIndex
        {
            get { return selectPageIndex; }
            set { selectPageIndex = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// T表单
        /// </summary>
        public T GridModel
        {
            get { return gridModel; }
            set { gridModel = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// T表单数据列表
        /// </summary>
        public ObservableCollection<T> GridModelList
        {
            get { return gridModelList; }
            set { gridModelList = value; RaisePropertyChanged(); }
        }

        public RelayCommand<string> ExecuteCommand { get; private set; }
        public RelayCommand QueryCommand { get; private set; }

        #region IDataOperation

        /// <summary>
        /// 新增
        /// </summary>
        public virtual void Add()
        {
            this.CreateDeaultCommand();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        public virtual void Edit()
        {
            this.CreateDeaultCommand();
        }

        public virtual void Save()
        {
            InitPermissions(this.AuthValue);
            SelectPageIndex = 0;
        }

        public virtual void Cancel()
        {
            InitPermissions(this.AuthValue);
            SelectPageIndex = 0;
        }

        /// <summary>
        /// 删除
        /// </summary>
        public virtual void Del() { }

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
        public virtual void Execute(string arg)
        {
            /*
             * 这里使用string来做弱类型处理,防止使用枚举,
             * 其他页面需要重新该方法
             */
            switch (arg)
            {
                case "添加": Add(); break;
                case "修改": Edit(); break;
                case "删除": Del(); break;
                case "保存": Save(); break;
                case "取消": Cancel(); break;
            }
        }

        /// <summary>
        /// 创建页面默认命令
        /// </summary>
        private void CreateDeaultCommand()
        {
            ToolBarCommandList.Clear();
            ToolBarCommandList.Add(new ButtonCommand() { CommandName = "保存", CommandColor = "#0066FF", CommandKind = "ContentSave" });
            ToolBarCommandList.Add(new ButtonCommand() { CommandName = "取消", CommandColor = "#FF6633", CommandKind = "Cancel" });
        }
        #endregion

        #endregion

        #region IDataPager (数据分页)
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
            await Task.FromResult(true);
        }

        /// <summary>
        /// 设置页数
        /// </summary>
        public virtual void SetPageCount()
        {
            PageCount = Convert.ToInt32(Math.Ceiling((double)TotalCount / (double)PageSize));
        }
        #endregion

        #region IAuthority

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

        #region Load event.

        public void UpdateLoading(bool isOpen, string msg = "")
        {
            Messenger.Default.Send(new MsgInfo()
            {
                IsOpen = isOpen,
                Msg = msg
            }, "UpdateDialog");
        }

        #endregion
    }
}
