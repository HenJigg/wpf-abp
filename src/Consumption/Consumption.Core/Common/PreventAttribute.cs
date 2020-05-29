/*
*
* 文件名    ：PreventAttribute                             
* 程序说明  : 禁止序列化特性
* 更新时间  : 2020-05-25 16：36
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/

namespace Consumption.Core.Common
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// 禁止序列化特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PreventAttribute : Attribute
    {
    }
}
