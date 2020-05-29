/*
*
* 文件名    ：UserRepository                             
* 程序说明  : 用户数据接口实现
* 更新时间  : 2020-05-21 16：41
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

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ConsumptionContext consumptionContext) : base(consumptionContext)
        {

        }
    }
}
