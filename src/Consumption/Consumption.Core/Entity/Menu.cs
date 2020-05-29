/*
*
* 文件名    ：Menu                             
* 程序说明  : 系统菜单实体
* 更新时间  : 2020-05-16 15:04
* 更新人    : zhouhaogg789@outlook.com
*
*
*/

namespace Consumption.Core.Entity
{
    using System.ComponentModel;

    /// <summary>
    /// 菜单
    /// </summary>
    public class Menu : BaseEntity
    {
        /// <summary>
        /// 菜单代码
        /// </summary>
        public string MenuCode { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单标题
        /// </summary>
        public string MenuCaption { get; set; }

        /// <summary>
        /// 命名空间
        /// </summary>
        public string MenuNameSpace { get; set; }

        /// <summary>
        /// 所属权限值
        /// </summary>
        [DefaultValue(0)]
        public int MenuAuth { get; set; }
    }
}
