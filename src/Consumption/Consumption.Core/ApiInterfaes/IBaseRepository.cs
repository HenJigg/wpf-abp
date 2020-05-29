/*
*
* 文件名    ：IBaseRepository                             
* 程序说明  : 基类接口, 具备普通得增删改查功能
* 更新时间  : 2020-05-21 11：15
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/

namespace Consumption.Core.ApiInterfaes
{
    using Consumption.Core.Common;
    using Consumption.Core.Query;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    public interface IBaseRepository<T> where T : class
    {
        Task<PaginatedList<T>> GetModelList(QueryParameters parameters);
        void AddModelAsync(T model);
        void UpdateModelAsync(T model);
        void DeleteModelAsync(T model);
    }
}
