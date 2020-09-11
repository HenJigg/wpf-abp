using System;
using System.Collections.Generic;
using System.Text;
/*
*
* 文件名    ：ButtonCommand                             
* 程序说明  : 动态按钮命令
* 更新时间  : 2020-07-10 15：03
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
    using GalaSoft.MvvmLight.Command;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// 按钮命令
    /// </summary>
    public class ButtonCommand
    {
        /// <summary>
        /// 命令名称
        /// </summary>
        public string CommandName { get; set; }

        /// <summary>
        /// 命令图类
        /// </summary>
        public string CommandKind { get; set; }

        /// <summary>
        /// 命令颜色
        /// </summary>
        public string CommandColor { get; set; }
    }
}
