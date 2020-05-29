/*
*
* 文件名    ：BaseRepository<T>                             
* 程序说明  : 基类数据接口实现
* 更新时间  : 2020-05-21 16：44
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/

namespace Consumption.EFCore.Repository
{
    using Consumption.Core.Common;
    using Consumption.Core.Query;
    using Consumption.EFCore.Orm;
    using Microsoft.EntityFrameworkCore.Migrations;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public class BaseRepository<T> where T : class
    {
        public readonly ConsumptionContext consumptionContext;

        public BaseRepository(ConsumptionContext consumptionContext)
        {
            this.consumptionContext = consumptionContext;
        }

        public Task<bool> AddModelAsync(T model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteModelAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetModelByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedList<T>> GetModelList(QueryParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateModelAsync(T model)
        {
            throw new NotImplementedException();
        }
    }
}
