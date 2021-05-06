/*
*
* 文件名    ：BasicController                          
* 程序说明  : 基础数据控制器
* 更新时间  : 2020-05-21 12:03 
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
    /// 基础数据控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BasicController : Controller
    {
        private readonly IBasicManager manager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        public BasicController(IBasicManager manager)
        {
            this.manager = manager;
        }


        /// <summary>
        /// 获取基础数据列表
        /// </summary>
        /// <param name="param">请求参数</param>
        /// <returns>结果</returns>
        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery] QueryParameters param) =>
          await manager.GetAll(param);

        /// <summary>
        /// 新增基础数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns>结果</returns>
        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] BasicDto param) =>
            await manager.Add(param);

        /// <summary>
        /// 更新基础数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResponse> Update([FromBody] BasicDto param) =>
              await manager.Save(param);

        /// <summary>
        /// 删除基础数数据
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>结果</returns>
        [HttpDelete]
        public async Task<ApiResponse> Delete(int id) =>
            await manager.Delete(id);
    }
}
