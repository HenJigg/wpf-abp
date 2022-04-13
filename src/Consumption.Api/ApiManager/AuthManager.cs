/*
*
* 文件名    ：AuthManager                          
* 程序说明  : 权限相关数据
* 更新时间  : 2020-05-21 11:44 
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
