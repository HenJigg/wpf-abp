
/*
*
* 文件名    ：UserLog                             
* 程序说明  : 用户日志实体
* 更新时间  : 2020-05-16 15:05
* 更新人    : zhouhaogg789@outlook.com
*
*
*/

namespace Consumption.Core.Entity
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
