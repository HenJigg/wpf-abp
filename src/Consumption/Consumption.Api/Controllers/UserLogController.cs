/*
*
* 文件名    ：UserLogController                     
* 程序说明  : 用户日志控制器
* 更新时间  : 2020-05-21 10：37
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Consumption.Core.Response;
    using Consumption.Core.Entity;
    using Consumption.Core.Query;
    using Consumption.EFCore;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// 用户日志控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserLogController : Controller
    {
        private readonly ILogger<UserLogController> logger;
        private readonly IUnitOfWork work;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repository"></param>
        /// <param name="work"></param>
        public UserLogController(ILogger<UserLogController> logger, IUnitOfWork work)
        {
            this.logger = logger;
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
                var models = await work.GetRepository<UserLog>().GetPagedListAsync(
                    predicate: x =>
                      string.IsNullOrWhiteSpace(parameters.Search) ? true : x.Content.Contains(parameters.Search) ||
                    string.IsNullOrWhiteSpace(parameters.Search) ? true : x.UserName.Contains(parameters.Search),
                    pageIndex: parameters.PageIndex,
                    pageSize: parameters.PageSize);
                return Ok(new ConsumptionResponse() { success = true, dynamicObj = models, TotalRecord = models.TotalCount });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
                return Ok(new ConsumptionResponse() { success = false, message = "Can't get data" });
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
                work.GetRepository<UserLog>().Insert(model);
                if (await work.SaveChangesAsync() > 0)
                    return Ok(new ConsumptionResponse() { success = true });
                return Ok(new ConsumptionResponse() { success = false, message = "Error saving data" });
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
                var repository = work.GetRepository<UserLog>();
                var user = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id == id);
                if (user == null)
                {
                    return Ok(new ConsumptionResponse() { success = false, message = "The user was not found!" });
                }
                repository.Delete(user);
                if (await work.SaveChangesAsync() > 0)
                    return Ok(new ConsumptionResponse() { success = true });
                return Ok(new ConsumptionResponse() { success = false, message = $"Deleting post {id} failed when saving." });
            }
            catch (Exception ex)
            {
                return Ok(new ConsumptionResponse() { success = false, message = ex.Message });
            }
        }
    }
}
