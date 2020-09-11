/*
*
* 文件名    ：Log                             
* 程序说明  : 日志接口, 实现普通的日志输出
* 更新时间  : 2020-09-10 10:05
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// 日志接口
    /// </summary>
    public interface ILog
    {
        void Error(Exception exception, string message);
        void Error(string message, params object[] args);
        void Error(string message);
        void Info(string message);
        void Info(string message, params object[] args);
        void Info(Exception exception, string message);
        void Warn(string message);
        void Warn(string message, params object[] args);
        void Warn(Exception exception, string message);
        void Debug(string message);
        void Debug(string message, params object[] args);
        void Debug(Exception exception, string message);
    }
}
