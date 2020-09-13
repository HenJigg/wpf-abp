/*
*
* 文件名    ：GlobalLoger                             
* 程序说明  : 日志记录器
* 更新时间  : 2020-09-10 11:05
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
    using Consumption.Common.Contract;
    using Consumption.Core.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


    /// <summary>
    /// 全局日志
    /// </summary>
    [Aspect(Scope.Global)]
    [Injection(typeof(GlobalLoger))]
    public class GlobalLoger : Attribute
    {
        private readonly ILog log;

        public GlobalLoger()
        {
            this.log = NetCoreProvider.Get<ILog>();
        }

        [Advice(Kind.Before, Targets = Target.Method)]
        public void Start([Argument(Source.Name)] string methodName, [Argument(Source.Arguments)] object[] arg)
        {
            log.Debug($"开始调用方法:{methodName},参数:{string.Join(",", arg)}");
        }

        [Advice(Kind.After, Targets = Target.Method)]
        public void End([Argument(Source.Name)] string methodName, [Argument(Source.ReturnValue)] object arg)
        {
            log.Debug($"结束调用方法:{methodName},返回值:{arg}");
        }
    }
}
