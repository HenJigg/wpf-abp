/*
* 文件名    ：BaseServiceRequest<T>                             
* 程序说明  : 请求服务基类
* 更新时间  : 2020-05-21 16:47 
*/

namespace Consumption.Service
{
    using Consumption.Shared.Common;
    using Consumption.Shared.HttpContact;
    using Newtonsoft.Json;
    using RestSharp;
    using System.Threading.Tasks;

    /// <summary>
    /// 请求服务基类
    /// </summary>
    public class BaseServiceRequest
    {
        private readonly string _requestUrl = Contract.serverUrl;

        public string requestUrl
        {
            get { return _requestUrl; }
        }

        /// <summary>
        /// restSharp实例
        /// </summary>
        public RestSharpCertificateMethod restSharp = new RestSharpCertificateMethod();

        /// <summary>
        /// T请求
        /// </summary>
        /// <param name="request">请求参数</param>
        /// <param name="method">方法类型</param>
        /// <returns></returns>
        public async Task<WebResult<T>> GetRequest<T>(BaseRequest request, Method method) where T : class
        {
            string pms = request.GetPropertiesObject();
            string url = requestUrl + request.route;
            if (!string.IsNullOrWhiteSpace(request.getParameter))
                url += request.getParameter;
            return await restSharp.RequestBehavior<T>(url, method, pms);
        }

        /// <summary>
        /// 请求
        /// </summary>
        /// <typeparam name="Response"></typeparam>
        /// <param name="url">地址</param>
        /// <param name="pms">参数</param>
        /// <param name="method">方法类型</param>
        /// <returns></returns>
        public async Task<WebResult<T>> GetRequest<T>(string route, object obj, Method method) where T : class
        {
            string pms = string.Empty;
            if (!string.IsNullOrWhiteSpace(obj?.ToString())) pms = JsonConvert.SerializeObject(obj);
            return await restSharp.RequestBehavior<T>(requestUrl + route, method, pms);
        }
    }
}
