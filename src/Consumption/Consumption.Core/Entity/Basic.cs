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
        /// <summary>
        /// 字典类型代码
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// 数据编号
        /// </summary>
        public string DataCode { get; set; }

        /// <summary>
        /// 中文名
        /// </summary>
        public string NativeName { get; set; }

        /// <summary>
        /// 英文名
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// 最后更新人
        /// </summary>
        public string LastUpdateBy { get; set; }
    }
}
