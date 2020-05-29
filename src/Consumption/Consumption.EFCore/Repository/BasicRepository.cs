/*
*
* 文件名    ：BasicRepository                             
* 程序说明  : 基础数据数据接口实现
* 更新时间  : 2020-05-21 16：43
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/


namespace Consumption.EFCore.Repository
{
    using Consumption.Core.ApiInterfaes;
    using Consumption.Core.Entity;
    using Consumption.EFCore.Orm;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public class BasicRepository : BaseRepository<Basic>, IBasicRepository
    {
        public BasicRepository(ConsumptionContext consumptionContext) : base(consumptionContext)
        {
        }

        public async Task<Basic> GetBasicByIdAsync(int id)
        {
            return await consumptionContext.Basics.FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
