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
    using Newtonsoft.Json.Linq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 数据库上下文帮助类
    /// </summary>
    public class ConsumptionHelper
    {
        /// <summary>
        /// 初始化样本数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task InitSampleDataAsync(ConsumptionContext context)
        {
            //做个简陋的判断,认为数据库没初始化
            if (!context.Users.Any())
            {
                //加载一些系统默认需要的数据

                List<User> userList = new List<User>();
                userList.Add(new User() { Account = "Admin", UserName = "管理员", Address = "Guangzhou", Tel = "1870620584", Password = "123", FlagAdmin = 1, CreateTime = DateTime.Now });
                userList.Add(new User() { Account = "Abigail", UserName = "愛比蓋爾", Address = "Guangzhou", Tel = "1870620584", Password = "123", CreateTime = DateTime.Now });
                userList.Add(new User() { Account = "Ada", UserName = "愛達", Address = "Guangzhou", Tel = "1870620584", Password = "123", CreateTime = DateTime.Now });
                userList.Add(new User() { Account = "Agatha", UserName = "阿加莎", Address = "Guangzhou", Tel = "1870620584", Password = "123", CreateTime = DateTime.Now });
                userList.Add(new User() { Account = "Diana", UserName = "黛安娜", Address = "Guangzhou", Tel = "1870620584", Password = "123", CreateTime = DateTime.Now });
                userList.Add(new User() { Account = "Eliza", UserName = "伊萊扎", Address = "Guangzhou", Tel = "1870620584", Password = "123", CreateTime = DateTime.Now });
                userList.Add(new User() { Account = "Flora", UserName = "弗洛拉", Address = "Guangzhou", Tel = "1870620584", Password = "123", CreateTime = DateTime.Now });
                userList.ForEach(async arg => await context.Users.AddAsync(arg));

                List<Menu> menuList = new List<Menu>();
                menuList.Add(new Menu() { MenuCode = "1001", MenuName = "用户管理", MenuCaption = "AccountBox", MenuNameSpace = "UserCenter", MenuAuth = 7 });
                menuList.Add(new Menu() { MenuCode = "1002", MenuName = "权限管理", MenuCaption = "Group", MenuNameSpace = "GroupCenter", MenuAuth = 7 });
                menuList.Add(new Menu() { MenuCode = "1003", MenuName = "个性化", MenuCaption = "Palette", MenuNameSpace = "SkinCenter", MenuAuth = 8 });
                menuList.Add(new Menu() { MenuCode = "1004", MenuName = "仪表板", MenuCaption = "TelevisionGuide", MenuNameSpace = "DashboardCenter", MenuAuth = 8 });
                menuList.Add(new Menu() { MenuCode = "1005", MenuName = "菜单管理", MenuCaption = "Menu", MenuNameSpace = "MenuCenter", MenuAuth = 7 });
                for (int i = 0; i < menuList.Count; i++)
                    await context.Menus.AddAsync(menuList[i]);

                List<AuthItem> authLis = new List<AuthItem>();
                authLis.Add(new AuthItem() { AuthName = "添加", AuthValue = 1, AuthColor = "#0080FF", AuthKind = "PlaylistPlus" });
                authLis.Add(new AuthItem() { AuthName = "修改", AuthValue = 2, AuthColor = "#28CBA3", AuthKind = "PlaylistPlay" });
                authLis.Add(new AuthItem() { AuthName = "删除", AuthValue = 4, AuthColor = "#FF5370", AuthKind = "PlaylistRemove" });
                authLis.Add(new AuthItem() { AuthName = "查看", AuthValue = 8, AuthColor = "#FF5370", AuthKind = "FileDocumentBoxSearchOutline" });
                authLis.Add(new AuthItem() { AuthName = "打印", AuthValue = 16, AuthColor = "#FF5370", AuthKind = "LocalPrintShop" });
                authLis.Add(new AuthItem() { AuthName = "导入", AuthValue = 32, AuthColor = "#FF5370", AuthKind = "UploadOutline" });
                authLis.Add(new AuthItem() { AuthName = "导出", AuthValue = 64, AuthColor = "#FF5370", AuthKind = "DownloadOutline" });

                for (int i = 0; i < authLis.Count; i++)
                    await context.AuthItems.AddAsync(authLis[i]);

                await context.SaveChangesAsync();
            }
        }
    }
}
