/*
*
* 文件名    ：MenuController                          
* 程序说明  : 菜单数据控制器
* 更新时间  : 2020-05-21 11:43 
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
