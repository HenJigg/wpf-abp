/*
*
* 文件名    ：UserController                            
* 程序说明  : 用户数据控制器
* 更新时间  : 2020-05-21 10：35
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
