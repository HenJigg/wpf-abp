using System;
using System.Collections.Generic;
using System.Text;

namespace Consumption.Core.Aop
{
    [AttributeUsage(AttributeTargets.Method)]
    public class GlabalLogerAttribute : Attribute
    {
        public GlabalLogerAttribute()
        {
        }
    }
}
