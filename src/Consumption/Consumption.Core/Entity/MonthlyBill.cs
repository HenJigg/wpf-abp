/*
*
* 文件名    ：MonthlyBill                             
* 程序说明  : 月度账单
* 更新时间  : 2020-05-22 16：02
* 更新人    : zhouhaogg789@outlook.com
*
*
* 作用      : 用于存储每个月统计得账单数据, 便于每次统计生成无需在查询结果
*/

using System;

namespace Consumption.Core.Entity
{
    /// <summary>
    /// 月度账单
    /// </summary>
    public class MonthlyBill : BaseEntity
    {
        /// <summary>
        /// 账单流水号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
