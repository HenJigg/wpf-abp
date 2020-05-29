/*
*
* 文件名    ：UserLogController                     
* 程序说明  : 用户日志控制器
* 更新时间  : 2020-05-215 10：37
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/

namespace Consumption.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Consumption.Core.ApiInterfaes;
    using Consumption.Core.Common;
    using Consumption.Core.Entity;
    using Consumption.Core.Query;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// 用户日志控制器
    /// </summary>
    public class UserLogController : Controller
    {
        private readonly ILogger<UserLogController> logger;
        private readonly IUserLogRepository repository;
        private readonly IUnitWork work;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repository"></param>
        /// <param name="work"></param>
        public UserLogController(ILogger<UserLogController> logger, IUserLogRepository repository, IUnitWork work)
        {
            this.logger = logger;
            this.repository = repository;
            this.work = work;
        }

        /// <summary>
        /// 获取用户日志数据信息
        /// </summary>
        /// <param name="parameters">请求参数</param>
        /// <returns>结果</returns>
        [HttpGet]
        public async Task<IActionResult> GetUserLogs([FromQuery] QueryParameters parameters)
        {
            try
            {
                var models = await repository.GetModelList(parameters);

                if (models.Count > 0)
                    return Ok(new ConsumptionResponse()
                    {
                        success = true,
                        dynamicObj = models,
                        TotalRecord = models.TotalCount
                    });
                return Ok();
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
        /// 新增用户日志
        /// </summary>
        /// <param name="model">用户信息</param>
        /// <returns>结果</returns>
        [HttpPost]
        public async Task<IActionResult> AddUserLog([FromBody] UserLog model)
        {
            try
            {
                if (model == null)
                {
                    return Ok(new ConsumptionResponse() { success = false, message = "Add data error" });
                }
                repository.AddModelAsync(model);
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
                return Ok(new ConsumptionResponse() { success = false, message = "Add userLog error" });
            }
        }

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>结果</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteUserLog(int id)
        {
            try
            {
                var user = await repository.GetUserLogByIdAsync(id);
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
