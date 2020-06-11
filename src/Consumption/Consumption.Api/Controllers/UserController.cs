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
*/

namespace Consumption.Api.Controllers
{
    using Consumption.Core.ApiInterfaes;
    using Consumption.Core.Common;
    using Consumption.Core.Entity;
    using Consumption.Core.Query;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 用户数据控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> logger;
        private readonly IUserRepository repository;
        private readonly IUnitWork work;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repository"></param>
        /// <param name="work"></param>
        public UserController(ILogger<UserController> logger,
            IUserRepository repository, IUnitWork work)
        {
            this.logger = logger;
            this.repository = repository;
            this.work = work;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="passWord">密码</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Login(string account, string passWord)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(passWord))
                    return Ok(new ConsumptionResponse() { success = false, message = "Request failed" });
                var model = await repository.LoginAsync(account, passWord);
                if (model != null)
                    return Ok(new ConsumptionResponse()
                    {
                        success = true,
                        dynamicObj = model
                    });
                else
                    return Ok(new ConsumptionResponse()
                    {
                        success = false,
                        message = "用户名或密码错误！"
                    });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
                return Ok(new ConsumptionResponse()
                {
                    success = false,
                    message = "Login failed"
                });
            }
        }

        /// <summary>
        /// 获取用户数据信息
        /// </summary>
        /// <param name="parameters">请求参数</param>
        /// <returns>结果</returns>
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] UserParameters parameters)
        {
            try
            {
                var models = await repository.GetModelList(parameters);

                return Ok(new ConsumptionResponse()
                {
                    success = true,
                    dynamicObj = models,
                    TotalRecord = models.TotalCount
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
                return Ok(new ConsumptionResponse()
                {
                    success = false,
                    message = "Can't get data"
                });
            }
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>结果</returns>
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            try
            {
                if (user == null)
                {
                    return Ok(new ConsumptionResponse() { success = false, message = "Add data error" });
                }
                user.CreateTime = DateTime.Now;
                user.LoginCounter = 0;
                user.IsLocked = 0;
                user.FlagAdmin = 0;
                repository.AddModelAsync(user);
                if (!await work.SaveChangedAsync())
                {
                    return Ok(new ConsumptionResponse()
                    {
                        success = false,
                        message = "Error saving data"
                    });
                }
                return Ok(new ConsumptionResponse() { success = true });
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex, "");
                return Ok(new ConsumptionResponse() { success = false, message = "Add user error" });
            }
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            if (user == null)
            {
                return Ok(new ConsumptionResponse() { success = false, message = "Update data error" });
            }
            try
            {
                var dbUser = await repository.GetUserByIdAsync(id);
                if (dbUser == null) return Ok(new ConsumptionResponse() { success = false, message = "The user was not found!" });
                dbUser.UserName = user.UserName;
                dbUser.Tel = user.Tel;
                dbUser.Password = user.Password;
                dbUser.IsLocked = user.IsLocked;
                dbUser.Address = user.Address;
                dbUser.Email = user.Email;
                dbUser.FlagAdmin = user.FlagAdmin;
                repository.UpdateModelAsync(dbUser);
                if (!await work.SaveChangedAsync())
                {
                    return Ok(new ConsumptionResponse() { success = false, message = $"update post {user.Id} failed when saving." });
                }
                return Ok(new ConsumptionResponse() { success = true });
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex, "");
                return Ok(new ConsumptionResponse() { success = false, message = "Update user error" });
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>结果</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await repository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return Ok(new ConsumptionResponse() { success = false, message = "The user was not found!" });
                }
                repository.DeleteModelAsync(user);
                if (!await work.SaveChangedAsync())
                {
                    return Ok(new ConsumptionResponse() { success = false, message = $"Deleting post {id} failed when saving." });
                }
                return Ok(new ConsumptionResponse() { success = true });
            }
            catch (Exception ex)
            {
                return Ok(new ConsumptionResponse() { success = false, message = ex.Message });
            }
        }
    }
}
