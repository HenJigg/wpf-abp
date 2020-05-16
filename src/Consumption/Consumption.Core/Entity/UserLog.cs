
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
        public string UserName { get; set; }

        public string Content { get; set; }

        public DateTime CreateTime { get; set; } 
    }
}
