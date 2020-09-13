/*
* 文件名    ：BaseService<T>                             
* 程序说明  : 具备基础的CRUD功能的请求积累
* 更新时间  : 2020-09-11 09:46
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
    using Consumption.Core.Request;
    using Consumption.Core.Response;
    using RestSharp;
    using System.Threading.Tasks;
    using Consumption.Core.Collections;
    using Consumption.Core.Query;

    public class BaseService<T>
    {
        public async Task<BaseResponse> AddAsync(T model)
        {
            var r = await new BaseServiceRequest().
             GetRequest<BaseResponse>($@"api/{typeof(T).Name}/Add", model, Method.POST);
            return r;
        }

        public async Task<BaseResponse> DeleteAsync(int id)
        {
            var r = await new BaseServiceRequest().
              GetRequest<BaseResponse>(@$"api/{typeof(T).Name}/Delete?id={id}", string.Empty, Method.DELETE);
            return r;
        }

        public async Task<BaseResponse<PagedList<T>>> GetAllListAsync(QueryParameters pms)
        {
            var r = await new BaseServiceRequest().
               GetRequest<BaseResponse<PagedList<T>>>(@$"api/{typeof(T).Name}/GetAll?PageIndex={pms.PageIndex}&PageSize={pms.PageSize}&Search={pms.Search}", string.Empty, Method.GET);
            return r;
        }

        public async Task<BaseResponse<T>> GetAsync(int id)
        {
            var r = await new BaseServiceRequest().
               GetRequest<BaseResponse<T>>(@$"api/{typeof(T).Name}/Get?id={id}", string.Empty, Method.GET);
            return r;
        }


        public async Task<BaseResponse> SaveAsync(T model)
        {
            var r = await new BaseServiceRequest().
                GetRequest<BaseResponse>($@"api/{typeof(T).Name}/Save", model, Method.POST);
            return r;
        }

        public async Task<BaseResponse<T>> UpdateAsync(T model)
        {
            var r = await new BaseServiceRequest().
                GetRequest<BaseResponse<T>>($@"api/{typeof(T).Name}/Update", model, Method.POST);
            return r;
        }
    }
}
