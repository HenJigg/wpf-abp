/*
*
* 文件名    ：PaginatedList                             
* 程序说明  : 数据分页基类
* 更新时间  : 2020-05-21 11：36
* 更新人    : zhouhaogg789@outlook.com
* 
*
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
