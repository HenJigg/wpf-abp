/*
*
* 文件名    ：DashboardCenter                             
* 程序说明  : 仪表板控制类 
* 更新时间  : 2020-06-07 17：37
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
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 仪表板控制类
    /// </summary>
    [Module("仪表板")]
    public class DashboardCenter : BaseCenter<DashboardView, DashboardViewModel>
    {

    }
}
