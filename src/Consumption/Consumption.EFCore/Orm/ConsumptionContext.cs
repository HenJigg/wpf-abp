/*
*
* 文件名    ：ConsumptionContext                             
* 程序说明  : 数据库上下文
* 更新时间  : 2020-05-21 14：17
* 更新人    : zhouhaogg789@outlook.com
*
* 
*/

namespace Consumption.EFCore.Orm
{
    using Consumption.Core.Entity;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Text;


    public class ConsumptionContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
           => options.UseSqlite("Data Source=note.db");

        public DbSet<User> Users { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<GroupFunc> GroupFuncs { get; set; }
        public DbSet<Basic> Basics { get; set; }
        public DbSet<BasicType> BasicTypes { get; set; }
        public DbSet<AuthItem> AuthItems { get; set; }
    }
}
