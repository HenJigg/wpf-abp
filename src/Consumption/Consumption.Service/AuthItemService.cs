/*
*
* 文件名    ：AuthItemService                             
* 程序说明  : 功能按钮清单服务
* 更新时间  : 2020-06-16 16：41
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

using Consumption.Core.Entity;
using Consumption.Core.Request;
using Consumption.Core.Response;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Consumption.Service
{
    public partial class ConsumptionService
    {
        /// <summary>
        /// 获取按钮清单
        /// </summary>
        /// <returns></returns>
        public async Task<BaseResponse<List<AuthItem>>> GetAuthListAsync()
        {
            BaseServiceRequest<BaseResponse<List<AuthItem>>> baseService =
               new BaseServiceRequest<BaseResponse<List<AuthItem>>>();
            var r = await baseService.GetRequest(new AuthItemRequest(), Method.GET);
            return r;
        }
    }
}
