/*
*
* 文件名    ：AuthItemController                          
* 程序说明  : 权限相关数据控制器
* 更新时间  : 2020-05-21 11:44
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
    using Consumption.EFCore;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// 权限相关数据控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthItemController : Controller
    {
        private readonly ILogger<AuthItemController> logger;
        private readonly IUnitOfWork work;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="work"></param>
        public AuthItemController(ILogger<AuthItemController> logger, IUnitOfWork work)
        {
            this.logger = logger;
            this.work = work;
        }

        /// <summary>
        /// 获取所有功能按钮列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAuthItems()
        {
            try
            {
                var models = await work.GetRepository<AuthItem>().GetAllAsync();
                if (models.Count > 0)
                {
                    return Ok(new ConsumptionResponse() { success = true, dynamicObj = models.OrderBy(t => t.AuthValue).ToList() });
                }
                return Ok(new ConsumptionResponse() { success = false, message="系统模块为空" });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
                return Ok(new ConsumptionResponse() { success = false, message = "获取功能清单异常" });
            }
        }
    }
}
