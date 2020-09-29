/*
*
* 文件名    ：Contract                             
* 程序说明  : 缓存信息
* 更新时间  : 2020-07-11 18：03
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.Shared.Common
{
    using Consumption.Shared.DataModel;
    using System.Collections.Generic;

    public static class Contract
    {
        #region 用户信息

        /// <summary>
        /// 登录名
        /// </summary>
        public static string Account = string.Empty;

        /// <summary>
        /// 用户名
        /// </summary>
        public static string UserName = string.Empty;

        /// <summary>
        /// 是否属于管理员
        /// </summary>
        public static bool IsAdmin;

        #endregion

        #region 权限验证信息

        /// <summary>
        /// 系统中已定义的功能清单-缓存用于页面验证
        /// </summary>
        public static List<AuthItem> AuthItems;

        /// <summary>
        /// 获取用户的所有模块
        /// </summary>
        public static List<Menu> Menus;

        #endregion

        #region 接口地址

        public static string serverUrl = string.Empty;

        #endregion
    }
}
