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

namespace Consumption.Core.Interfaces
{
    using Consumption.Core.Collections;
    using Consumption.Core.Entity;
    using Consumption.Core.Query;
    using Consumption.Core.Request;
    using Consumption.Core.Response;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IConsumptionRepository<T>
    {
        Task<BaseResponse<PagedList<T>>> GetAllListAsync(QueryParameters parameters);

        Task<BaseResponse<T>> GetAsync(int id);

        Task<BaseResponse> SaveAsync(T model);

        Task<BaseResponse> AddAsync(T model);

        Task<BaseResponse> DeleteAsync(int id);

        Task<BaseResponse<T>> UpdateAsync(T model);
    }

    public interface IUserRepository : IConsumptionRepository<User>
    {
        Task<BaseResponse<UserInfo>> LoginAsync(string account, string passWord);

        /// <summary>
        /// 获取用户的所属权限信息
        /// </summary>
        Task<BaseResponse<List<Menu>>> GetUserPermByAccountAsync(string account);

        Task<BaseResponse<List<AuthItem>>> GetAuthListAsync();
    }

    public interface IGroupRepository : IConsumptionRepository<Group>
    {
        /// <summary>
        /// 获取菜单模块列表(包含每个菜单拥有的一些功能)
        /// </summary>
        /// <returns></returns>
        Task<BaseResponse<List<MenuModuleGroup>>> GetMenuModuleListAsync();

        /// <summary>
        /// 根据ID获取用户组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<BaseResponse<GroupHeader>> GetGroupAsync(int id);

        /// <summary>
        /// 保存组数据
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task<BaseResponse> SaveGroupAsync(GroupHeader group);
    }

    public interface IMenuRepository : IConsumptionRepository<Menu>
    {

    }

    public interface IBasicRepository : IConsumptionRepository<Basic>
    {

    }

}
