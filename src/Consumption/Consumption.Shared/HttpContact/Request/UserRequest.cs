/*
*
* 文件名    ：UserLoginRequest                             
* 程序说明  : 用户请求
* 更新时间  : 2020-05-30 20：40
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.Core.Request
{
    using Consumption.Shared.Dto;
    using Consumption.Shared.HttpContact;

    /// <summary>
    /// 用户登录请求
    /// </summary>
    public class UserLoginRequest : BaseRequest
    {
        public override string route { get => "api/User/Login"; }

        public LoginDto Parameter { get; set; }
    }

    /// <summary>
    /// 用户权限请求
    /// </summary>
    public class UserPermRequest : BaseRequest
    {
        public override string route { get => "api/User/Perm"; }

        public string account { get; set; }
    }

}
