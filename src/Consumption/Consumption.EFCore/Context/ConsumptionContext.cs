/*
*
* 文件名    ：ConsumptionContext                             
* 程序说明  : 数据库上下文
* 更新时间  : 2020-05-21 14：17
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.EFCore.Context
{
    using Consumption.Core.Entity;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Text;


    public class ConsumptionContext : DbContext
    {
        public ConsumptionContext(DbContextOptions<ConsumptionContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<GroupFunc> GroupFuncs { get; set; }
        public DbSet<Basic> Basics { get; set; }
        public DbSet<BasicType> BasicTypes { get; set; }
        public DbSet<AuthItem> AuthItems { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<UserConfig> UserConfigs { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Quotes> Quotes { get; set; }
        public DbSet<MonthlyBill> MonthlyBills { get; set; }
        public DbSet<MonthlyBillDetail> MonthlyBillDetails { get; set; }

    }
}
