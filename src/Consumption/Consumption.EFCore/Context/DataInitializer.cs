/*
*
* 文件名    ：ConsumptionHelper                             
* 程序说明  : 数据库上下文帮助类
* 更新时间  : 2020-05-22 13：48
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/


namespace Consumption.EFCore.Context
{
    using Consumption.Core.Entity;
    using Consumption.Core.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 数据库帮助类
    /// </summary>
    public class DataInitializer : IDataInitializer
    {
        private readonly ILogger<DataInitializer> logger;
        private readonly ConsumptionContext context;
        public DataInitializer(ILogger<DataInitializer> logger, ConsumptionContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        /// <summary>
        /// 初始化数据库
        /// </summary>
        /// <returns></returns>
        public async Task InitSampleDataAsync()
        {
            await this.EnsureCreateAsync();
            await this.CreateSampleDataAsync();
        }

        async Task EnsureCreateAsync()
        {
            await context.Database.EnsureCreatedAsync();
        }

        async Task CreateSampleDataAsync()
        {
            if (!context.Users.Any() && !context.Menus.Any() && !context.AuthItems.Any())
            {
                context.Users.AddRange(
                    new User()
                    {
                        Account = "Diana",
                        UserName = "黛安娜",
                        Address = "Guangzhou",
                        Tel = "1870620584",
                        Password = "123",
                        CreateTime = DateTime.Now
                    },
                    new User()
                    {
                        Account = "Eliza",
                        UserName = "伊萊扎",
                        Address = "Guangzhou",
                        Tel = "1870620584",
                        Password = "123",
                        CreateTime = DateTime.Now
                    },
                    new User()
                    {
                        Account = "Flora",
                        UserName = "弗洛拉",
                        Address = "Guangzhou",
                        Tel = "1870620584",
                        Password = "123",
                        CreateTime = DateTime.Now
                    });

                context.Menus.AddRange(
                    new Menu() { MenuCode = "1001", MenuName = "用户管理", MenuCaption = "AccountBox", MenuNameSpace = "UserCenter", MenuAuth = 7 },
                    new Menu() { MenuCode = "1002", MenuName = "权限管理", MenuCaption = "Group", MenuNameSpace = "GroupCenter", MenuAuth = 7 },
                    new Menu() { MenuCode = "1003", MenuName = "个性化", MenuCaption = "Palette", MenuNameSpace = "SkinCenter", MenuAuth = 8 },
                    new Menu() { MenuCode = "1004", MenuName = "仪表板", MenuCaption = "TelevisionGuide", MenuNameSpace = "DashboardCenter", MenuAuth = 8 },
                    new Menu() { MenuCode = "1005", MenuName = "菜单管理", MenuCaption = "Menu", MenuNameSpace = "MenuCenter", MenuAuth = 7 }
                    );

                context.Menus.AddRange(
                    new Menu() { MenuCode = "1001", MenuName = "用户管理", MenuCaption = "AccountBox", MenuNameSpace = "UserCenter", MenuAuth = 7 },
                    new Menu() { MenuCode = "1002", MenuName = "权限管理", MenuCaption = "Group", MenuNameSpace = "GroupCenter", MenuAuth = 7 },
                    new Menu() { MenuCode = "1003", MenuName = "个性化", MenuCaption = "Palette", MenuNameSpace = "SkinCenter", MenuAuth = 8 },
                    new Menu() { MenuCode = "1004", MenuName = "仪表板", MenuCaption = "TelevisionGuide", MenuNameSpace = "DashboardCenter", MenuAuth = 8 },
                    new Menu() { MenuCode = "1005", MenuName = "菜单管理", MenuCaption = "Menu", MenuNameSpace = "MenuCenter", MenuAuth = 7 }
                    );

                await context.SaveChangesAsync();
            }
        }
    }
}
