/*
*
* 文件名    ：AuthItemController                            
* 程序说明  : 权限相关数据控制器
* 更新时间  : 2020-05-21 10：35 
*/

namespace Consumption.Api.Controllers
{
    using System.Threading.Tasks;
    using Consumption.Api.ApiManager;
    using Consumption.Shared.HttpContact.Response;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// 权限相关数据控制器
    /// </summary>
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthItemController : Controller
    {
        private readonly IAuthItemManager manager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="manager"></param>
        public AuthItemController(IAuthItemManager manager)
        {
            this.manager = manager;
        }

        /// <summary>
        /// 获取所有功能按钮列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResponse> GetAll()=>
            await manager.GetAll();
    }
}
