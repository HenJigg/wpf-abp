/*
*
* 文件名    ：IUserRepository                             
* 程序说明  : 用户数据接口
* 更新时间  : 2020-05-21 11：00
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/

using Consumption.Core.Entity;
using System.Threading.Tasks;

namespace Consumption.Core.ApiInterfaes
{
    public interface IUserRepository : IBaseRepository<User>
    {

    }
}
