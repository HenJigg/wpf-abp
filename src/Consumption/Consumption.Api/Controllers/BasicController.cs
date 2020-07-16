/*
*
* 文件名    ：BasicController                          
* 程序说明  : 基础数据控制器
* 更新时间  : 2020-05-21 12:03
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
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using Consumption.Core.Response;
    using Consumption.Core.Entity;
    using Consumption.Core.Query;
    using Consumption.EFCore;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// 基础数据控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BasicController : Controller
    {
        private readonly ILogger<BasicController> logger;
        private readonly IUnitOfWork work;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="work"></param>
        public BasicController(ILogger<BasicController> logger, IUnitOfWork work)
        {
            this.logger = logger;
            this.work = work;
        }


        /// <summary>
        /// 获取基础数据列表
        /// </summary>
        /// <param name="parameters">请求参数</param>
        /// <returns>结果</returns>
        [HttpGet]
        public async Task<IActionResult> GetBasics([FromQuery] QueryParameters parameters)
        {
            try
            {
                var models = await work.GetRepository<Basic>().GetPagedListAsync(
                    predicate: x =>
                    string.IsNullOrWhiteSpace(parameters.Search) ? true : x.DataCode.Contains(parameters.Search),
                    pageIndex: parameters.PageIndex,
                    pageSize: parameters.PageSize);
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
        /// 新增基础数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns>结果</returns>
        [HttpPost]
        public async Task<IActionResult> AddBasic([FromBody] Basic model)
        {
            try
            {
                if (model == null)
                    return Ok(new ConsumptionResponse() { success = false, message = "Add data error" });
                await work.GetRepository<Basic>().InsertAsync(model);
                if (await work.SaveChangesAsync() > 0)
                    return Ok(new ConsumptionResponse() { success = true });
                return Ok(new ConsumptionResponse() { success = false, message = "Error saving data" });
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex, "");
                return Ok(new ConsumptionResponse() { success = false, message = "Add basic error" });
            }
        }

        /// <summary>
        /// 更新基础数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="model">用户信息</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdateBasic(int id, [FromBody] Basic model)
        {
            if (model == null)
                return Ok(new ConsumptionResponse() { success = false, message = "Update data error" });
            try
            {
                var repository = work.GetRepository<Basic>();
                var dbmodel = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id == id);
                if (dbmodel == null) return Ok(new ConsumptionResponse() { success = false, message = "The basic was not found!" });
                dbmodel.NativeName = model.NativeName;
                dbmodel.EnglishName = model.EnglishName;
                dbmodel.LastUpdate = model.LastUpdate;
                dbmodel.DataCode = model.DataCode;
                dbmodel.LastUpdateBy = model.LastUpdateBy;
                repository.Update(dbmodel);
                if (await work.SaveChangesAsync() > 0)
                    return Ok(new ConsumptionResponse() { success = true });
                return Ok(new ConsumptionResponse() { success = false, message = $"update post {id} failed when saving." });
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex, "");
                return Ok(new ConsumptionResponse() { success = false, message = "Update basic error" });
            }
        }

        /// <summary>
        /// 删除基础数数据
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>结果</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteBasic(int id)
        {
            try
            {
                var repository = work.GetRepository<Basic>();
                var user = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id == id);
                if (user == null)
                    return Ok(new ConsumptionResponse() { success = false, message = "The basic was not found!" });
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
