/*
*
* 文件名    ：IBasicRepository                             
* 程序说明  : 基础数据接口
* 更新时间  : 2020-05-21 10：58
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/

using Consumption.Core.Entity;
using System.Threading.Tasks;

namespace Consumption.Core.ApiInterfaes
{
    public interface IBasicRepository : IBaseRepository<Basic>
    {
        Task<Basic> GetBasicByIdAsync(int id);
    }
}
