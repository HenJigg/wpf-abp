/*
*
* 文件名    ：AuthManager                          
* 程序说明  : 权限相关数据
* 更新时间  : 2020-05-21 11:44
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
* 
* 最后更新时间: 2020-09-29 11：31
* 更新说明: 重命名
*/

namespace Consumption.Api.ApiManager
{
    using Consumption.EFCore;
    using Consumption.Shared.DataModel;
    using Consumption.Shared.HttpContact.Response;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class AuthManager : IAuthItemManager
    {
        private readonly ILogger<AuthManager> logger;
        private readonly IUnitOfWork work;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="work"></param>
        public AuthManager(ILogger<AuthManager> logger, IUnitOfWork work)
        {
            this.logger = logger;
            this.work = work;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse> GetAll()
        {
            try
            {
                var models = await work.GetRepository<AuthItem>().GetAllAsync();
                return new ApiResponse(200, models.OrderBy(t => t.AuthValue).ToList());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "");
                return new ApiResponse(201, "");
            }
        }

    }
}
