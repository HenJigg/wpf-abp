/*
*
* 文件名    ：IUserRepository                             
* 程序说明  : 用户数据接口
* 更新时间  : 2020-05-21 11：00
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/


namespace Consumption.Core.ApiInterfaes
{

    using Consumption.Core.Entity;
    using System.Threading.Tasks;

    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUserByIdAsync(int id);
    }
}
