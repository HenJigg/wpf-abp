namespace Consumption.Shared.DataModel
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
