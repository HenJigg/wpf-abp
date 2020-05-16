/*
*
* 文件名    ：Notify.cs                              
* 程序说明  : 窗口提示类型
* 更新时间  : 2020-05-11
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/
namespace Consumption.Core.Enums
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.ComponentModel;

    /// <summary>
    /// 提示类型
    /// </summary>
    public enum Notify
    {
        /// <summary>
        /// 错误
        /// </summary>
        [Description("错误")]
        Error,
        /// <summary>
        /// 警告
        /// </summary>
        [Description("警告")]
        Warning,
        /// <summary>
        /// 提示信息
        /// </summary>
        [Description("提示信息")]
        Info,
        /// <summary>
        /// 询问信息
        /// </summary>
        [Description("询问信息")]
        Question,
    }
}
