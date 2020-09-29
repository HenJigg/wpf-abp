/*
* 文件名    ：BaseService<T>                             
* 程序说明  : 具备基础的CRUD功能的请求基类
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
    using Consumption.Shared.Common.Collections;
    using Consumption.Shared.Common.Query;
    using Consumption.Shared.HttpContact;
    using RestSharp;
    using System.Threading.Tasks;

    public class BaseService<T>
    {
        private readonly string servicesName;
        public BaseService()
        {
            servicesName = typeof(T).Name.Replace("Dto", string.Empty);
        }
        public async Task<BaseResponse> AddAsync(T model)
        {
            var r = await new BaseServiceRequest().
             GetRequest<BaseResponse>($@"api/{servicesName}/Add", model, Method.POST);
            return r;
        }

        public async Task<BaseResponse> DeleteAsync(int id)
        {
            var r = await new BaseServiceRequest().
              GetRequest<BaseResponse>(@$"api/{servicesName}/Delete?id={id}", string.Empty, Method.DELETE);
            return r;
        }

        public async Task<BaseResponse> GetAllListAsync(QueryParameters pms)
        {
            var r = await new BaseServiceRequest().
               GetRequest<BaseResponse>(@$"api/{servicesName}/GetAll?PageIndex={pms.PageIndex}&PageSize={pms.PageSize}&Search={pms.Search}", string.Empty, Method.GET);
            return r;
        }

        public async Task<BaseResponse> GetAsync(int id)
        {
            var r = await new BaseServiceRequest().
               GetRequest<BaseResponse>(@$"api/{servicesName}/Get?id={id}", string.Empty, Method.GET);
            return r;
        }


        public async Task<BaseResponse> SaveAsync(T model)
        {
            var r = await new BaseServiceRequest().
                GetRequest<BaseResponse>($@"api/{servicesName}/Save", model, Method.POST);
            return r;
        }

        public async Task<BaseResponse> UpdateAsync(T model)
        {
            var r = await new BaseServiceRequest().
                GetRequest<BaseResponse>($@"api/{servicesName}/Update", model, Method.POST);
            return r;
        }
    }
}
