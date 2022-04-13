/*
* 文件名    ：BaseService<T>                             
* 程序说明  : 具备基础的CRUD功能的请求基类
* 更新时间  : 2020-09-11 09:46 
*/

namespace Consumption.Service
{
    using Consumption.Shared.Common.Collections;
    using Consumption.Shared.Common.Query;
    using Consumption.Shared.HttpContact;
    using RestSharp;
    using System.Threading.Tasks;

    public class BaseService<T> where T : class
    {
        private readonly string servicesName;
        public BaseService()
        {
            servicesName = typeof(T).Name.Replace("Dto", string.Empty);
        }
        public async Task<WebResult> AddAsync(T model)
        {
            var r = await new BaseServiceRequest().
             GetRequest<WebResult>($@"api/{servicesName}/Add", model, Method.POST);
            return r;
        }

        public async Task<WebResult> DeleteAsync(int id)
        {
            var r = await new BaseServiceRequest().
              GetRequest<WebResult>(@$"api/{servicesName}/Delete?id={id}", string.Empty, Method.DELETE);
            return r;
        }

        public async Task<WebResult<PagedList<T>>> GetAllListAsync(QueryParameters pms)
        {
            var r = await new BaseServiceRequest().
               GetRequest<PagedList<T>>(@$"api/{servicesName}/GetAll?PageIndex={pms.PageIndex}&PageSize={pms.PageSize}&Search={pms.Search}", string.Empty, Method.GET);
            return r;
        }

        public async Task<WebResult<T>> GetAsync(int id)
        {
            var r = await new BaseServiceRequest().
               GetRequest<T>(@$"api/{servicesName}/Get?id={id}", string.Empty, Method.GET);
            return r;
        }


        public async Task<WebResult> SaveAsync(T model)
        {
            var r = await new BaseServiceRequest().
                GetRequest<WebResult>($@"api/{servicesName}/Save", model, Method.POST);
            return r;
        }

        public async Task<WebResult> UpdateAsync(T model)
        {
            var r = await new BaseServiceRequest().
                GetRequest<WebResult>($@"api/{servicesName}/Update", model, Method.POST);
            return r;
        }
    }
}
