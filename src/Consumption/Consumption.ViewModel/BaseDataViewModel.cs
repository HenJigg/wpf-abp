/*
*
* 文件名    ：BaseDataViewModel                             
* 程序说明  : 数据视图基类-GUID
* 更新时间  : 2020-06-08 22：29
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
    using Consumption.Core.Interfaces;
    using Consumption.ViewModel.Common;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 基础数据基类
    /// 
    /// 用于:
    ///   1. 实现基础的数据列表展示功能
    ///   2. 实现基础的增删改查单页功能
    ///   3. 实现基础页面的单页数据分页功能
    /// </summary>
    public class BaseDataViewModel<T> : ViewModelBase, IDataPager where T : class, new()
    {
        public BaseDataViewModel()
        {
            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand<T>(t => Edit(t));
            DelCommand = new RelayCommand<T>(t => Del(t));
            QueryCommand = new RelayCommand(Query);
            SwitchModeCommand = new RelayCommand<bool>(arg =>
              {
                  DisplayType = arg;
              });
        }

        private bool displayType = true;

        public bool DisplayType
        {
            get { return displayType; }
            set { displayType = value; RaisePropertyChanged(); }
        }

        public RelayCommand<bool> SwitchModeCommand { get; private set; }

        #region GUID

        private string searchText = string.Empty;
        private ObservableCollection<T> gridModelList;

        /// <summary>
        /// 搜索内容
        /// </summary>
        public string SearchText
        {
            get { return searchText; }
            set { searchText = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 抽象表单数据
        /// </summary>
        public ObservableCollection<T> GridModelList
        {
            get { return gridModelList; }
            set { gridModelList = value; RaisePropertyChanged(); }
        }

        public RelayCommand AddCommand { get; private set; }
        public RelayCommand<T> EditCommand { get; private set; }
        public RelayCommand<T> DelCommand { get; private set; }
        public RelayCommand QueryCommand { get; private set; }

        #region IDataOperation

        /// <summary>
        /// 新增
        /// </summary>
        public virtual void Add() { }

        /// <summary>
        /// 编辑
        /// </summary>
        public virtual void Edit<TModel>(TModel model) { }

        /// <summary>
        /// 删除
        /// </summary>
        public virtual void Del<TModel>(TModel model) { }

        /// <summary>
        /// 查询
        /// </summary>
        public virtual void Query()
        {
            this.GetPageData(this.PageIndex);
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
            if (this.PageIndex == 1) return;
            PageIndex = 1;
            await GetPageData(PageIndex);
        }

        /// <summary>
        /// 上一页
        /// </summary>
        public virtual async void GoOnPage()
        {
            if (this.PageIndex == 1) return;
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
        public virtual Task GetPageData(int pageIndex)
        {
            return default;
        }

        /// <summary>
        /// 设置页数
        /// </summary>
        public virtual void SetPageCount()
        {
            PageCount = Convert.ToInt32(Math.Ceiling((double)TotalCount / (double)PageSize));
        }
        #endregion
    }
}
