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

        public async Task<BaseResponse<PagedList<T>>> GetAllListAsync(QueryParameters pms)
        {
            var r = await new BaseServiceRequest().
               GetRequest<BaseResponse<PagedList<T>>>(@$"api/{servicesName}/GetAll?PageIndex={pms.PageIndex}&PageSize={pms.PageSize}&Search={pms.Search}", string.Empty, Method.GET);
            return r;
        }

        public async Task<BaseResponse<T>> GetAsync(int id)
        {
            var r = await new BaseServiceRequest().
               GetRequest<BaseResponse<T>>(@$"api/{servicesName}/Get?id={id}", string.Empty, Method.GET);
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
