/*
*
* 文件名    ：AuthItemController                            
* 程序说明  : 权限相关数据控制器
* 更新时间  : 2020-05-21 10：35
* 更新人    : zhouhaogg789@outlook.com
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
* 
* 最后更新时间: 2020-09-29
* 更新内容: 转移业务代码离开控制器层
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
