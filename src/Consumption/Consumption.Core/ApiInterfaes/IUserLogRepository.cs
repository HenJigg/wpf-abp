/*
*
* 文件名    ：IUserLogRepository                             
* 程序说明  : 用户日志数据接口
* 更新时间  : 2020-05-21 11：00
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/

namespace Consumption.Core.ApiInterfaes
{

    using Consumption.Core.Entity;
    using System.Threading.Tasks;

    public interface IUserLogRepository : IBaseRepository<UserLog>
    {
        Task<UserLog> GetUserLogByIdAsync(int id);
    }
}
