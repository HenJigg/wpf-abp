/*
*
* 文件名    ：MenuRepository                             
* 程序说明  : 菜单数据接口实现
* 更新时间  : 2020-05-21 11:52
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

    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        public MenuRepository(ConsumptionContext consumptionContext) : base(consumptionContext)
        {
        }

        public async Task<Menu> GetMenuByIdAsync(int id)
        {
            return await consumptionContext.Menus.FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}
