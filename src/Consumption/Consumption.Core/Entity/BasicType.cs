/*
*
* 文件名    ：BasicType                             
* 程序说明  : 字典类型实体
* 更新时间  : 2020-05-16 15:02
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/

namespace Consumption.Core.Entity
{
    /// <summary>
    /// 字典类型
    /// </summary>
    public class BasicType : BaseEntity
    {
        /// <summary>
        /// 字典代码
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// 字典名称
        /// </summary>
        public string TypeName { get; set; }
    }
}
