/*
*
* 文件名    ：Log                             
* 程序说明  : 日志管理器
* 更新时间  : 2020-07-08 12:49
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.Core.Common
{
    using NLog;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Log
    {
        private static NLog.Logger logger;
        static Log()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg"></param>
        public static void Error(string msg)
        {
            logger.Error(msg);
        }
        
        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="msg"></param>
        public static void Info(string msg)
        {
            logger.Info(msg);
        }

        /// <summary>
        /// 测试信息
        /// </summary>
        /// <param name="msg"></param>
        public static void Debug(string msg)
        {
            logger.Debug(msg);
        }
    }
}
