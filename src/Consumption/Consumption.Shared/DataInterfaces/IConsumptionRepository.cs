/*
*
* 文件名    ：IConsumptionService                             
* 程序说明  : 系统接口管理层
* 更新时间  : 2020-06-27 12：56
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
    using Consumption.Shared.Common.Collections;
    using Consumption.Shared.Common.Query;
    using Consumption.Shared.DataModel;
    using Consumption.Shared.Dto;
    using Consumption.Shared.HttpContact;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IConsumptionRepository<T>
    {
        Task<BaseResponse<PagedList<T>>> GetAllListAsync(QueryParameters parameters);

        Task<BaseResponse<T>> GetAsync(int id);

        Task<BaseResponse> SaveAsync(T model);

        Task<BaseResponse> AddAsync(T model);

        Task<BaseResponse> DeleteAsync(int id);

        Task<BaseResponse> UpdateAsync(T model);
    }

    public interface IUserRepository : IConsumptionRepository<UserDto>
    {
        Task<BaseResponse<UserInfoDto>> LoginAsync(string account, string passWord);

        /// <summary>
        /// 获取用户的所属权限信息
        /// </summary>
        Task<BaseResponse> GetUserPermByAccountAsync(string account);

        Task<BaseResponse<List<AuthItem>>> GetAuthListAsync();
    }

    public interface IGroupRepository : IConsumptionRepository<GroupDto>
    {
        /// <summary>
        /// 获取菜单模块列表(包含每个菜单拥有的一些功能)
        /// </summary>
        /// <returns></returns>
        Task<BaseResponse<List<MenuModuleGroupDto>>> GetMenuModuleListAsync();

        /// <summary>
        /// 根据ID获取用户组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<GroupDataDto>> GetGroupAsync(int id);

        /// <summary>
        /// 保存组数据
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task<BaseResponse> SaveGroupAsync(GroupDataDto group);
    }

    public interface IMenuRepository : IConsumptionRepository<MenuDto>
    {

    }

    public interface IBasicRepository : IConsumptionRepository<BasicDto>
    {

    }

}
