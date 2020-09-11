using System;
using System.Collections.Generic;
using System.Text;

namespace Consumption.Core.Aop
{
    [AttributeUsage(AttributeTargets.Method)]
    public class GlabalProgressAttribute:Attribute
    {
        /// <summary>
        /// 窗口提示内容
        /// </summary>
        /// <param name="message"></param>
        public GlabalProgressAttribute(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
