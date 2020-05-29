/*
*
* 文件名    ：Bill                             
* 程序说明  : 账单实体模型
* 更新时间  : 2020-05-22 15：41
* 更新人    : zhouhaogg789@outlook.com
*
*
*/

namespace Consumption.Core.Entity
{
    /// <summary>
    /// 账单
    /// </summary>
    public class Bill : BaseEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 消费类型
        /// </summary>
        public int ConsumptionType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public string UpdateDate { get; set; }
    }
}
