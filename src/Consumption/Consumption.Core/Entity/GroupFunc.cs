/*
*
* 文件名    ：GroupFunc                             
* 程序说明  : 用户组所对应功能实体
* 更新时间  : 2020-05-16 15:03
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/

namespace Consumption.Core.Entity
{
    using System.ComponentModel;

    /// <summary>
    /// 组功能
    /// </summary>
    public class GroupFunc : BaseEntity
    {
        public string GroupCode { get; set; }

        public string MenuCode { get; set; }

        [DefaultValue(0)]
        public int Auth { get; set; }
    }
}
