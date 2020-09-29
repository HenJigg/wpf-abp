/*
*
* 文件名    ：UserViewModel                             
* 程序说明  : 用户信息
* 更新时间  : 2020-07-26 17:18
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
* 
* 最后更新内容: 添加用户的增删改查
*/


namespace Consumption.ViewModel
{
    using Consumption.Shared.Common;
    using Consumption.Shared.DataModel;
    using Consumption.Shared.Dto;
    using Consumption.ViewModel.Interfaces;

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserViewModel : BaseRepository<UserDto>, IUserViewModel
    {
        public UserViewModel() : base(NetCoreProvider.Get<IUserRepository>())
        {

        }
    }
}
