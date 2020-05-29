/*
*
* 文件名    ：AuthItemController                          
* 程序说明  : 权限相关数据控制器
* 更新时间  : 2020-05-215 11:44
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
        private readonly IAuthItemRepository repository;
        private readonly IUnitWork work;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="repository"></param>
        /// <param name="work"></param>
        public AuthItemController(ILogger<AuthItemController> logger,
            IAuthItemRepository repository, IUnitWork work)
        {
            this.logger = logger;
            this.repository = repository;
            this.work = work;
        }
    }
}
