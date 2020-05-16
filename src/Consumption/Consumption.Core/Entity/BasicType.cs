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
        public string TypeCode { get; set; }

        public string TypeName { get; set; }
    }
}
