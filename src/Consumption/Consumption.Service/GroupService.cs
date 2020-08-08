/*
*
* 文件名    ：ConsumptionService                             
* 程序说明  : 用户组服务
* 更新时间  : 2020-08-04 17：22
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
    using Consumption.Core.RequestForm;
    using Consumption.Core.Response;
    using RestSharp;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// 用户组服务
    /// </summary>
    public partial class ConsumptionService
    {
        public async Task<BaseResponse<PagedList<Group>>> GetGroupListAsync(QueryParameters parameters)
        {
            BaseServiceRequest<BaseResponse<PagedList<Group>>> baseService =
               new BaseServiceRequest<BaseResponse<PagedList<Group>>>();
            var r = await baseService.GetRequest(new GroupRequest()
            {
                parameters = parameters
            }, Method.GET);
            return r;
        }

        public async Task<BaseResponse<List<MenuModuleGroup>>> GetMenuModuleListAsync()
        {
            BaseServiceRequest<BaseResponse<List<MenuModuleGroup>>> baseService =
              new BaseServiceRequest<BaseResponse<List<MenuModuleGroup>>>();
            var r = await baseService.GetRequest(new GroupModuleRequest(), Method.GET);
            return r;
        }

        public async Task<BaseResponse<GroupHeader>> GetGroupAsync(int id)
        {
            BaseServiceRequest<BaseResponse<GroupHeader>> baseService =
              new BaseServiceRequest<BaseResponse<GroupHeader>>();
            var r = await baseService.GetRequest(new GroupInfoRequest() { id = id }, Method.GET);
            return r;
        }

        public async Task<BaseResponse> SaveGroupAsync(GroupHeader group)
        {
            BaseServiceRequest<BaseResponse> baseService =
              new BaseServiceRequest<BaseResponse>();
            var r = await baseService.GetRequest(new GroupSaveRequest()
            { parameter = group }, Method.POST);
            return r;
        }
    }
}
