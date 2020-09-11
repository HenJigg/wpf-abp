using Autofac.Extras.DynamicProxy;
using Consumption.Core.Aop;
using Consumption.Core.Entity;
using Consumption.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Consumption.ViewModel.Interfaces
{
    /// <summary>
    /// 实现基础的增删改查、分页、权限接口
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseViewModel<TEntity> : ICrud<TEntity>, IDataPager, IAuthority where TEntity : BaseEntity
    { }

    public interface IUserViewModel : IBaseViewModel<User>
    { }

    public interface IGroupViewModel : IBaseViewModel<Group>
    { }

    public interface IMenuViewModel : IBaseViewModel<Menu>
    { }

    public interface IBasicViewModel : IBaseViewModel<Basic>
    { }


}
