/*
*
* 文件名    ：User                             
* 程序说明  : 用户实体
* 更新时间  : 2020-05-16 15:05
* 更新人    : zhouhaogg789@outlook.com
*
*
*/

namespace Consumption.Core.Entity
{
    using System;

    /// <summary>
    /// 用户
    /// </summary>
    public class User : BaseEntity
    {
        public string Account { get; set; }

        public string UserName { get; set; }

        public string Address { get; set; }

        public string Tel { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public DateTime LastLoginTime { get; set; }

        public DateTime LastLogouTime { get; set; }

        public int IsLocked { get; set; }

        public DateTime CreateTime { get; set; }

        public string FlagAdmin { get; set; }

        public string FlagOnline { get; set; }

        public int LoginCounter { get; set; }
    }
}
