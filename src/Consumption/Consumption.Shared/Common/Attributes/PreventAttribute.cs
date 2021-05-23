namespace Consumption.Shared.Common.Attributes
{
    using System;

    /// <summary>
    /// 禁止序列化特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PreventAttribute : Attribute
    {  }
}
