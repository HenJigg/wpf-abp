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
    using Consumption.Core.RequestForm;
    using Consumption.Core.Response;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    /*  《接口层命名规范》
     * 1.模块接口统一定义至 IConsumptionService 中
     * 2.接口请求命名规范:
     *   2.1. Get<T>Async 
     *   2.2. Update<T>Async
     *   2.3. Delete<T>Async
     *   2.4. Put<T>Async
     * 3.接口带参数请求命名规范:
     *   3.1. 根据ID请求, 对应的 Get<T>ByIdAsync...
     * 4.接口带实体请求规范:
     *   4.1. 添加与更新类型请求,在实体中根据ID来区分请求类型..
     *   
     * 注: 在单个程序集当中的接口, 不再区分多个接口。
     *     所有模块按程序集来区分,程序集中按命名规范来区分。
     */

    public interface IConsumptionService
    {
        #region 基础数据接口

        Task<BaseResponse<PagedList<Basic>>> GetBasicListAsync(QueryParameters parameters);

        #endregion

        #region 菜单接口

        Task<BaseResponse<PagedList<Menu>>> GetMenuListAsync(QueryParameters parameters);

        #endregion

        #region 用户接口

        Task<BaseResponse<UserInfo>> LoginAsync(string account, string passWord);

        Task<BaseResponse<PagedList<User>>> GetUserListAsync(UserParameters parameters);

        Task<BaseResponse<User>> GetUserAsync(int id);

        Task<BaseResponse> SaveUserAsync(User user);

        Task<BaseResponse> DeleteUserAsync(int id);

        /// <summary>
        /// 获取用户的所属权限信息
        /// </summary>
        Task<BaseResponse<List<Menu>>> GetUserPermByAccountAsync(string account);

        #endregion

        #region 功能按钮接口

        Task<BaseResponse<List<AuthItem>>> GetAuthListAsync();

        #endregion

        #region 用户组/权限接口

        /// <summary>
        /// 获取用户组数据
        /// </summary>
        Task<BaseResponse<PagedList<Group>>> GetGroupListAsync(QueryParameters parameters);

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

        #endregion
    }
}
