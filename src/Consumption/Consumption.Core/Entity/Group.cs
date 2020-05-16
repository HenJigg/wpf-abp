/*
*
* 文件名    ：Group                             
* 程序说明  : 用户组实体
* 更新时间  : 2020-05-16 15:03
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/

namespace Consumption.Core.Entity
{
    /// <summary>
    /// 组
    /// </summary>
    public class Group : BaseEntity
    {
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
    }
}
