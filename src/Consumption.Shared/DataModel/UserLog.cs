namespace Consumption.Shared.DataModel
{
    using System;

    /// <summary>
    /// 用户日志实体
    /// </summary>
    public class UserLog : BaseEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } 
    }
}
