/*
*
* 文件名    ：MonthlyBillDetail                             
* 程序说明  : 月度账单明细
* 更新时间  : 2020-05-22 16：06
* 更新人    : zhouhaogg789@outlook.com
*
*
* 作用      : 用于存储每个月统计得账单数据明细信息
*/

namespace Consumption.Core.Entity
{
    /// <summary>
    /// 月度账单明细
    /// </summary>
    public class MonthlyBillDetail : BaseEntity
    {
        /// <summary>
        /// 主表流水号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 消费类型
        /// </summary>
        public int ConsumptionType { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
