/*
*
* 文件名    ：GroupUser                             
* 程序说明  : 用户组所对应用户实体
* 更新时间  : 2020-05-16 15:03
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/

namespace Consumption.Core.Entity
{
    /// <summary>
    /// 组用户
    /// </summary>
    public class GroupUser : BaseEntity
    {
        public string GroupCode { get; set; }

        public string Account { get; set; }
    }
}
