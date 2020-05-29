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
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UserLogRepository : BaseRepository<UserLog>, IUserLogRepository
    {
        public UserLogRepository(ConsumptionContext consumptionContext) : base(consumptionContext)
        {
        }
    }
}
