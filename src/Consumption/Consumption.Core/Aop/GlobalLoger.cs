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
    using Castle.DynamicProxy;
    using Consumption.Core.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;


    /// <summary>
    /// 日志记录器
    /// </summary>
    public class GlobalLoger : IInterceptor
    {
        private readonly ILog log;

        public GlobalLoger(ILog log)
        {
            this.log = log;
        }

        public void Intercept(IInvocation invocation)
        {
            bool isEnable = false;
            var attr = invocation.Method.GetCustomAttributes(typeof(GlabalLogerAttribute), false);
            if (attr != null && attr.Length > 0)
            {
                isEnable = true;
            }
            if (isEnable)
                log.Debug($"我轻轻的来:{invocation.Method.Name},参数:{string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())}");
            invocation.Proceed();
            if (isEnable)
                log.Debug($"正如我轻轻的走:{invocation.Method.Name},返回结果:{invocation.ReturnValue}");
        }
    }
}
