/*
*
* 文件名    ：IBaseViewModel                             
* 程序说明  : ViewModel的实现接口
* 更新时间  : 2020-09-12 11:35
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/


namespace Consumption.ViewModel.Interfaces
{
    using Consumption.Shared.DataInterfaces;
    using Consumption.Shared.DataModel;
    using Consumption.Shared.Dto;
    using Microsoft.Toolkit.Mvvm.ComponentModel;
    using Microsoft.Toolkit.Mvvm.Input;
    using System.Threading.Tasks;

    #region 模块接口

    /// <summary>
    /// 实现基础的增删改查、分页、权限接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseViewModel<TEntity> : IOrdinary<TEntity>, IDataPager, IAuthority where TEntity : class
    { }

    public interface IUserViewModel : IBaseViewModel<UserDto>
    { }

    public interface IGroupViewModel : IBaseViewModel<GroupDto>
    { }

    public interface IMenuViewModel : IBaseViewModel<MenuDto>
    { }

    public interface IBasicViewModel : IBaseViewModel<BasicDto>
    { }

    #endregion

    #region 组件接口

    public interface IComponentViewModel { }
    public interface ISkinViewModel : IComponentViewModel { }
    public interface IDashboardViewModel : IComponentViewModel { }
    public interface IHomeViewModel : IComponentViewModel { }
    public interface ILoginViewModel : IBaseDialog { }
    public interface IMainViewModel : IBaseDialog
    {
        Task InitDefaultView();
    }

    #endregion

    #region ICenter

    public interface ILoginCenter
    {
        Task<bool> ShowDialog();
    }
    public interface IMainCenter
    {
        Task<bool> ShowDialog();
    }
    public interface IUserCenter : IBaseCenter { }
    public interface IMenuCenter : IBaseCenter { }
    public interface IGroupCenter : IBaseCenter { }
    public interface IBasicCenter : IBaseCenter { }
    public interface IHomeCenter : IBaseCenter { }
    public interface IDashboardCenter : IBaseCenter { }
    public interface ISkinCenter : IBaseCenter { }
    public interface IMsgCenter
    {
        Task<bool> Show(object obj);
    }

    #endregion

    /// <summary>
    /// 弹窗窗口基础接口
    /// </summary>
    public interface IBaseDialog
    {
        bool DialogIsOpen { get; set; }

        void SnackBar(string msg);

        RelayCommand ExitCommand { get; }
    }
}
