/*
*
* 文件名    ：BaseRepository<T>                             
* 程序说明  : 基类数据接口实现
* 更新时间  : 2020-05-21 16：44
* 更新人    : zhouhaogg789@outlook.com
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
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
