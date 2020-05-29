/*
*
* 文件名    ：Plan                             
* 程序说明  : 消费计划
* 更新时间  : 2020-05-16 16：22
* 更新人    : zhouhaogg789@outlook.com
*
* 作用      : 用于配置单个用户的消费计划, 可以提前设定消费的内容与消费时间。
*             
*               
*/

namespace Consumption.Core.Entity
{

    using System;

    /// <summary>
    /// 消费计划
    /// </summary>
    public class Plan : BaseEntity
    {
        /// <summary>
        /// 计划标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 计划主题
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 计划金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 计划时间
        /// </summary>
        public DateTime PlanDate { get; set; }

        /// <summary>
        /// 计划状态
        /// </summary>
        public int PlanStatus { get; set; }
    }
}
