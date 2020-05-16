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
        public string MenuCode { get; set; }

        public string MenuName { get; set; }

        public string MenuCaption { get; set; }

        public string MenuNameSpace { get; set; }

        [DefaultValue(0)]
        public int MenuAuth { get; set; }
    }
}
