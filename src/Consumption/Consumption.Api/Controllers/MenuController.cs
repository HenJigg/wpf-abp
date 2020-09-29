/*
*
* 文件名    ：MenuController                          
* 程序说明  : 菜单数据控制器
* 更新时间  : 2020-05-21 11:43
* 更新人    : zhouhaogg789@outlook.com
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
* 
* 最后更新时间: 2020-09-29
* 更新内容: 转移业务代码离开控制器层
*/

namespace Consumption.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Consumption.Api.ApiManager;
    using Consumption.EFCore;
    using Consumption.Shared.Common.Query;
    using Consumption.Shared.DataModel;
    using Consumption.Shared.Dto;
    using Consumption.Shared.HttpContact.Response;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// 菜单数据控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class MenuController : Controller
    {
        private readonly IMenuManager manager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        public MenuController(IMenuManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="parameters">请求参数</param>
        /// <returns>结果</returns>
        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery] QueryParameters parameters) =>
            await manager.GetAll(parameters) ;

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="param">用户信息</param>
        /// <returns>结果</returns>
        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] MenuDto param) =>
             await manager.Add(param);

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>结果</returns>
        [HttpDelete]
        public async Task<ApiResponse> Delete(int id)=>
            await manager.Delete(id);
    }
}
