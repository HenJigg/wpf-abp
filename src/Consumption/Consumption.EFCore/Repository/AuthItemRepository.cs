/*
*
* 文件名    ：AuthItemRepository                             
* 程序说明  : 权限数据接口实现
* 更新时间  : 2020-05-21 16：45
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/


namespace Consumption.EFCore.Repository
{
    using Consumption.Core.ApiInterfaes;
    using Consumption.Core.Entity;
    using Consumption.EFCore.Orm;
    public class AuthItemRepository : BaseRepository<AuthItem>, IAuthItemRepository
    {
        public AuthItemRepository(ConsumptionContext consumptionContext) : base(consumptionContext)
        {

        }
    }
}
