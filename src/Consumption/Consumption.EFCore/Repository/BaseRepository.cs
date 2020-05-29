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
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Migrations;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BaseRepository<T> where T : class
    {
        public readonly ConsumptionContext consumptionContext;

        public BaseRepository(ConsumptionContext consumptionContext)
        {
            this.consumptionContext = consumptionContext;
        }

        public void AddModelAsync(T model)
        {
            consumptionContext.Entry<T>(model).State = EntityState.Added;
        }

        public void DeleteModelAsync(T model)
        {
            consumptionContext.Entry<T>(model).State = EntityState.Deleted;
        }

        public async Task<PaginatedList<T>> GetModelList(QueryParameters parameters)
        {
            var query = consumptionContext.Set<T>().AsQueryable();
            int count = await query.CountAsync();
            var data = await query.Skip((parameters.PageIndex - 1) *
                parameters.PageSize).Take(parameters.PageSize).ToListAsync();
            return new PaginatedList<T>(parameters.PageIndex - 1,
                parameters.PageSize, count, data);
        }

        public void UpdateModelAsync(T model)
        {
            consumptionContext.Entry<T>(model).State = EntityState.Modified;
        }
    }
}
