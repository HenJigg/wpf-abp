/*
*
* 文件名    ：BasicService                             
* 程序说明  : 基础数据服务
* 更新时间  : 2020-06-16 16：41
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.Service
{
    using Consumption.Core.Collections;
    using Consumption.Core.Entity;
    using Consumption.Core.Query;
    using Consumption.Core.Request;
    using Consumption.Core.Response;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 基础数据服务
    /// </summary>
    public partial class ConsumptionService
    {
        public async Task<BaseResponse<PagedList<Basic>>> GetBasicListAsync(QueryParameters parameters)
        {
            BaseServiceRequest<BaseResponse<PagedList<Basic>>> baseService =
            new BaseServiceRequest<BaseResponse<PagedList<Basic>>>();
            var r = await baseService.GetRequest(new BasicQueryRequest()
            {
                parameters = parameters
            }, Method.GET);
            return r;
        }
    }
}
