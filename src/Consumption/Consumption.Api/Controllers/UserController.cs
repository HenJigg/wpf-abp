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

namespace Consumption.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly IUserRepository repository;
        private readonly IUnitWork work;

        public UserController(ILogger<UserController> logger,
            IUserRepository repository, IUnitWork work)
        {
            this.logger = logger;
            this.repository = repository;
            this.work = work;
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
                var users = await repository.GetModelList(parameters);

                if (users.Count > 0)
                    return Ok(new ConsumptionResponse()
                    {
                        success = true,
                        dynamicObj = users,
                        TotalRecord = users.TotalCount
                    });
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
                return Ok(new ConsumptionResponse()
                {
                    success = false,
                    message = "无法获取数据"
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
                    return Ok(new ConsumptionResponse() { success = false, message = "添加的数据有误" });
                }
                repository.AddModelAsync(user);
                if (!await work.SaveChangedAsync())
                {
                    return Ok(new ConsumptionResponse()
                    {
                        success = false,
                        message = "保存数据错误"
                    });
                }
                return Ok(new ConsumptionResponse() { success = true });
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex, "");
                return Ok(new ConsumptionResponse() { success = false, message = "添加用户出错" });
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
                return Ok(new ConsumptionResponse() { success = false, message = "更新的数据有误" });
            }
            try
            {
                var dbUser = await repository.GetUserByIdAsync(id);
                if (dbUser == null) return Ok(new ConsumptionResponse() { success = false, message = "未能找到该用户信息!" });
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
                return Ok(new ConsumptionResponse() { success = false, message = "更新用户出错" });
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
                    return Ok(new ConsumptionResponse() { success = false, message = "未能找到该用户信息!" });
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
