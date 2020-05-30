/*
*
* 文件名    ：PaginatedList                             
* 程序说明  : 数据分页基类
* 更新时间  : 2020-05-21 11：36
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.Core.Common
{
    using System.Collections.Generic;

    public class PaginatedList<T> : List<T> where T : class
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        private int _totalCount;
        public int TotalCount
        {
            get => _totalCount;
            set => _totalCount = value >= 0 ? value : 0;
        }

        public int PageCount => _totalCount / PageSize + (_totalCount % PageSize > 0 ? 1 : 0);

        public PaginatedList(int pageIndex, int pageSize, int totalItemsCount, IEnumerable<T> data)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            _totalCount = totalItemsCount;
            AddRange(data);
        }
    }
}
