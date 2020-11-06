/*
*
* 文件名    ：UserCenter                             
* 程序说明  : 用户管理控制类 
* 更新时间  : 2020-06-03 20:22
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.PC.ViewCenter
{
    using Consumption.PC.View;
    using Consumption.PC.Common;
    using System.Windows.Controls;
    using Consumption.ViewModel.Interfaces;
    using Consumption.Shared.Common.Enums;
    using Consumption.Shared.Common.Attributes;
    using Consumption.Shared.Dto;
    using Consumption.Shared.Common;

    /// <summary>
    /// 用户管理类
    /// </summary>
    [Module("用户管理", ModuleType.系统配置)]
    public class UserCenter : ModuleCenter<UserView, UserDto>, IUserCenter
    {
        public UserCenter(IUserViewModel viewModel) : base(viewModel)
        { }
        public override void BindDataGridColumns()
        {
            VisualHelper.SetDataGridColumns<UserControl>(view, "Grid", typeof(UserDto));
        }
    }
}
