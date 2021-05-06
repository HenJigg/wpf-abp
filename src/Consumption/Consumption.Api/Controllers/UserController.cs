/*
*
* 文件名    ：UserController                            
* 程序说明  : 用户数据控制器
* 更新时间  : 2020-05-21 10：35 
*/

namespace Consumption.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using Consumption.Shared.HttpContact.Response;
    using Consumption.Shared.Common.Query;
    using Consumption.Api.ApiManager;
    using Consumption.Shared.Dto;
    using Microsoft.AspNetCore.Authorization;

    /// <summary>
    /// 用户数据控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly IUserManager manager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        public UserController(IUserManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResponse> Login(LoginDto param) =>
           await manager.Login(param);

        /// <summary>
        /// 获取用户数据信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResponse> Get(int id) =>
            await manager.Get(id);

        /// <summary>
        /// 获取用户数据列表信息
        /// </summary>
        /// <param name="parameters">请求参数</param>
        /// <returns>结果</returns>
        [HttpGet]
        public async Task<ApiResponse> GetAll([FromQuery] UserParameters parameters) =>
            await manager.GetAll(parameters);


        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResponse> Add([FromBody] UserDto param) =>
            await manager.Add(param);

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResponse> Save([FromBody] UserDto param) =>
             await manager.Save(param);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>结果</returns>
        [HttpDelete]
        public async Task<ApiResponse> Delete(int id) =>
            await manager.Delete(id);

    }
}
