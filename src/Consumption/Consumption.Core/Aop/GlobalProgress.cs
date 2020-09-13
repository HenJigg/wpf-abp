/*
*
* 文件名    ：GlobalProgress                             
* 程序说明  : 全局进度 
* 更新时间  : 2020-09-11 20：42
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.Core.Aop
{
    using AspectInjector.Broker;
    using Consumption.Core.Common;
    using GalaSoft.MvvmLight.Messaging;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 全局进度
    /// </summary>
    [Aspect(Scope.Global)]
    [Injection(typeof(GlobalProgress))]
    public class GlobalProgress : Attribute
    {
        [Advice(Kind.Before, Targets = Target.Method)]
        public void Start([Argument(Source.Name)] string name)
        {
            UpdateLoading(true);
        }

        [Advice(Kind.After, Targets = Target.Method)]
        public async void End([Argument(Source.Name)] string name)
        {
            await Task.Delay(300);
            UpdateLoading(false);
        }

        void UpdateLoading(bool isOpen, string msg = "Loading...")
        {
            Messenger.Default.Send(new MsgInfo()
            {
                IsOpen = isOpen,
                Msg = msg
            }, "UpdateDialog");
        }
    }
}
