using Abp.Application.Services.Dto;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AppFramework.Shared.Services
{
    public class PageIndexChangedEventArgs : EventArgs
    {
        public int OldPageIndex { get; internal set; }

        public int NewPageIndex { get; internal set; }

        public int SkipCount { get; internal set; }

        public int PageSize { get; internal set; }
    }

    public delegate void PageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e);

    /// <summary>
    /// 数据分页服务接口
    /// </summary>
    public interface IDataPagerService
    {
        /// <summary>
        /// 当前页索引
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// 页面大小
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        int PageCount { get; set; }

        /// <summary>
        /// 选中项
        /// </summary>
        object SelectedItem { get; set; }

        /// <summary>
        /// 分页按钮数量
        /// </summary>
        int NumericButtonCount { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        ObservableCollection<object> GridModelList { get; set; }

        /// <summary>
        /// 设置数据源(分页)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        Task SetList<T>(IPagedResult<T> pagedResult);

        /// <summary>
        /// 设置数据源(集合)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listResult"></param>
        /// <returns></returns>
        Task SetList<T>(IListResult<T> listResult);

        /// <summary>
        /// 页面索引改变事件
        /// </summary>
        event PageIndexChangedEventhandler OnPageIndexChangedEventhandler;
    }
}
