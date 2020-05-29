/*
*
* 文件名    ：AuthItem                             
* 程序说明  : 权限值定义实体
* 更新时间  : 2020-05-16 15:01
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/

namespace Consumption.Core.Entity
{
    /// <summary>
    /// 权限
    /// </summary>
    public class AuthItem : BaseEntity
    {
        /// <summary>
        /// 权限定义名称
        /// </summary>
        public string AuthName { get; set; }

        /// <summary>
        /// 所属权限值
        /// </summary>

        public string AuthValue { get; set; }
    }
}
