/*
*
* 文件名    ：UserLogRepository                             
* 程序说明  : 用户日志数据接口实现
* 更新时间  : 2020-05-21 16：42
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

    public class UserLogRepository : BaseRepository<UserLog>, IUserLogRepository
    {
        public UserLogRepository(ConsumptionContext consumptionContext) : base(consumptionContext)
        {
        }

        public async Task<UserLog> GetUserLogByIdAsync(int id)
        {
            return await consumptionContext.UserLogs.FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
