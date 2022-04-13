namespace Consumption.ViewModel.Common
{
    using Consumption.Shared.DataInterfaces;
    using Consumption.Shared.Dto;

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
    public interface ILoginViewModel : IBaseDialog { }

    #endregion

    /// <summary>
    /// 弹窗窗口基础接口
    /// </summary>
    public interface IBaseDialog
    {
        bool DialogIsOpen { get; set; }

        void SnackBar(string msg);
    }
}
