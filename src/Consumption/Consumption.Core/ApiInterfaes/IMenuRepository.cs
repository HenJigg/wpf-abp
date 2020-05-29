/*
*
* 文件名    ：IMenuRepository                             
* 程序说明  : 菜单数据接口
* 更新时间  : 2020-05-21 11:51
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/


namespace Consumption.Core.ApiInterfaes
{
    using Consumption.Core.Entity;
    using System.Threading.Tasks;

    public interface IMenuRepository : IBaseRepository<Menu>
    {
        Task<Menu> GetMenuByIdAsync(int id);
    }
}
