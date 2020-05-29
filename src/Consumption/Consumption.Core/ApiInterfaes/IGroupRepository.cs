/*
*
* 文件名    ：IGroupRepository                             
* 程序说明  : 用户组类型接口
* 更新时间  : 2020-05-21 11:56
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/



namespace Consumption.Core.ApiInterfaes
{
    using Consumption.Core.Entity;
    using System.Threading.Tasks;

    public interface IGroupRepository : IBaseRepository<Group>
    {
        Task<Group> GetGroupByIdAsync(int id);
    }
}
