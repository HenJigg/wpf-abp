/*
*
* 文件名    ：Basic                             
* 程序说明  : 字典类实体
* 更新时间  : 2020-05-16 15:02
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/

namespace Consumption.Core.Entity
{
    using System;
    
    /// <summary>
    /// 字典
    /// </summary>
    public class Basic : BaseEntity
    {
        public string TypeCode { get; set; }

        public string DataCode { get; set; }

        public string NativeName { get; set; }

        public string EnglishName { get; set; }

        public DateTime CreationDate { get; set; }

        public string CreateBy { get; set; }

        public DateTime LastUpdate { get; set; }

        public string LastUpdateBy { get; set; }
    }
}
