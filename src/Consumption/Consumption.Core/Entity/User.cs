/*
*
* 文件名    ：User                             
* 程序说明  : 用户实体
* 更新时间  : 2020-05-16 15:05
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.Core.Entity
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// 用户
    /// </summary>
    public partial class User : BaseEntity
    {
        /// <summary>
        /// 账户名称
        /// </summary>
        [Description("账户名称")]
        public string Account { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Description("用户名")]
        public string UserName { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Description("地址")]
        public string Address { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Description("电话")]
        public string Tel { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Description("邮箱")]
        public string Email { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }

        /// <summary>
        /// 最后登出时间
        /// </summary>
        public DateTime LastLogouTime { get; set; }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public int IsLocked { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否为管理员  0/1 false/true
        /// </summary>
        public int FlagAdmin { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public string FlagOnline { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        public int LoginCounter { get; set; }
    }

    public partial class User
    {
        private bool isChecked;

        /// <summary>
        /// 是否选中
        /// </summary>
        [NotMapped]
        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; RaisePropertyChanged(); }
        }
    }
}
