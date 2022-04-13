/*
*
* 文件名    ：GroupController                          
* 程序说明  : 组相关数据控制器
* 更新时间  : 2020-05-21 11:43 
*/

namespace Consumption.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Consumption.EFCore;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using NLog.Fluent;
    using System.Collections.ObjectModel;
    using Consumption.Shared.Common.Query;
    using Consumption.Shared.HttpContact.Response;
    using Consumption.Shared.DataModel;
    using Consumption.Shared.Dto;
    using Consumption.Api.ApiManager;

    /// <summary>
    ///  组控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GroupController : Controller
    {
        private readonly IGroupManager manager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        public GroupController(IGroupManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// 获取组列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery] QueryParameters param) =>
            await manager.GetAll(param);

        /// <summary>
        /// 保存组数据(新增/更新)
        /// </summary>
        /// <param name="model">组数据</param>
        /// <returns>结果</returns>
        [HttpPost]
        public async Task<ApiResponse> Save([FromBody] GroupDataDto model) =>
            await manager.Save(model);


        /// <summary>
        /// 删除组
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>结果</returns>
        [HttpDelete]
        public async Task<ApiResponse> Delete(int id) =>
            await manager.Delete(id);

        /// <summary>
        /// 获取菜单模块列表(包含每个菜单拥有的一些功能)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResponse> GetMenuModules() =>
            await manager.GetMenuModuleList();

        /// <summary>
        /// 查询组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResponse> GetGroupInfo(int id) =>
            await manager.GetGroupData(id);

    }
}
