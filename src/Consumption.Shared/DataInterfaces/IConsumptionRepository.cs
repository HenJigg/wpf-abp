namespace Consumption.ViewModel.Interfaces
{
    using Consumption.Shared.Common.Collections;
    using Consumption.Shared.Common.Query;
    using Consumption.Shared.DataModel;
    using Consumption.Shared.Dto;
    using Consumption.Shared.HttpContact;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IConsumptionRepository<T>
    {
        Task<WebResult<PagedList<T>>> GetAllListAsync(QueryParameters parameters);

        Task<WebResult<T>> GetAsync(int id);

        Task<WebResult> SaveAsync(T model);

        Task<WebResult> AddAsync(T model);

        Task<WebResult> DeleteAsync(int id);

        Task<WebResult> UpdateAsync(T model);
    }

    public interface IUserRepository : IConsumptionRepository<UserDto>
    {
        Task<WebResult<UserInfoDto>> LoginAsync(string account, string passWord);

        /// <summary>
        /// 获取用户的所属权限信息
        /// </summary>
        Task<WebResult> GetUserPermByAccountAsync(string account);

        Task<WebResult<List<AuthItem>>> GetAuthListAsync();
    }

    public interface IGroupRepository : IConsumptionRepository<GroupDto>
    {
        /// <summary>
        /// 获取菜单模块列表(包含每个菜单拥有的一些功能)
        /// </summary>
        /// <returns></returns>
        Task<WebResult<List<MenuModuleGroupDto>>> GetMenuModuleListAsync();

        /// <summary>
        /// 根据ID获取用户组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WebResult<GroupDataDto>> GetGroupAsync(int id);

        /// <summary>
        /// 保存组数据
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task<WebResult> SaveGroupAsync(GroupDataDto group);
    }

    public interface IMenuRepository : IConsumptionRepository<MenuDto>
    {

    }

    public interface IBasicRepository : IConsumptionRepository<BasicDto>
    {

    }

}
