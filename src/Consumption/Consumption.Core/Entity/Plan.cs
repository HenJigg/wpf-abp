/*
*
* 文件名    ：Plan                             
* 程序说明  : 消费计划
* 更新时间  : 2020-05-16 16：22
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
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
