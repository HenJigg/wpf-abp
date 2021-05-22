/*
*
* 文件名    ：ConsumptionHelper                             
* 程序说明  : 数据库上下文帮助类
* 更新时间  : 2020-05-22 13：48
* 更新人    : zhouhaogg789@outlook.com
*/

namespace Consumption.EFCore.Context
{
    using Consumption.Shared.DataInterfaces;
    using Consumption.Shared.DataModel;
    using Microsoft.Extensions.Logging;
    using System;
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
            context.Database.EnsureCreated();
            await this.CreateSampleDataAsync();
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
                        CreateTime = DateTime.Now,
                        FlagAdmin = 1,
                    },
                    new User()
                    {
                        Account = "Eliza",
                        UserName = "伊萊扎",
                        Address = "Guangzhou",
                        Tel = "1870620584",
                        Password = "123",
                        CreateTime = DateTime.Now,
                        FlagAdmin = 1,
                    },
                    new User()
                    {
                        Account = "Admin",
                        UserName = "弗洛拉",
                        Address = "Guangzhou",
                        Tel = "1870620584",
                        Password = "123",
                        CreateTime = DateTime.Now,
                        FlagAdmin = 1,
                    });

                context.Menus.AddRange(
                    new Menu() { MenuCode = "1001", MenuName = "用户管理", MenuCaption = "AccountBox", MenuNameSpace = "UserView", MenuAuth = 7 },
                    new Menu() { MenuCode = "1002", MenuName = "权限管理", MenuCaption = "Group", MenuNameSpace = "GroupView", MenuAuth = 7 },
                    new Menu() { MenuCode = "1003", MenuName = "个性化", MenuCaption = "Palette", MenuNameSpace = "SkinView", MenuAuth = 8 },
                    new Menu() { MenuCode = "1005", MenuName = "菜单管理", MenuCaption = "Menu", MenuNameSpace = "MenuView", MenuAuth = 7 });

                context.AuthItems.AddRange(
                    new AuthItem() { AuthColor = "#0080FF", AuthKind = "PlaylistPlus", AuthName = "添加", AuthValue = 1 },
                    new AuthItem() { AuthColor = "#28CBA3", AuthKind = "PlaylistPlay", AuthName = "修改", AuthValue = 2 },
                    new AuthItem() { AuthColor = "#FF5370", AuthKind = "PlaylistRemove", AuthName = "删除", AuthValue = 4 },
                    new AuthItem() { AuthColor = "#FF5370", AuthKind = "FileDocumentBoxSearchOutline", AuthName = "查看", AuthValue = 8 },
                    new AuthItem() { AuthColor = "#FF5370", AuthKind = "LocalPrintShop", AuthName = "打印", AuthValue = 16 },
                    new AuthItem() { AuthColor = "#FF5370", AuthKind = "UploadOutline", AuthName = "导入", AuthValue = 32 },
                    new AuthItem() { AuthColor = "#FF5370", AuthKind = "DownloadOutline", AuthName = "导出", AuthValue = 64 }
                    );

                await context.SaveChangesAsync();
            }
        }
    }
}
