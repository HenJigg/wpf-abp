/*
*
* 文件名    ：PlanCenter                             
* 程序说明  : 计划管理控制类 
* 更新时间  : 2020-06-03 20：23
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.PC.ViewCenter
{
    using Consumption.Core.Attributes;
    using Consumption.PC.View;
    using Consumption.ViewModel;
    using Consumption.ViewModel.Common;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 计划管理控制类
    /// </summary>
    //[Module("计划管理")]
    public class PlanCenter : BaseCenter<PlanView, PlanViewModel>
    {
    }
}
