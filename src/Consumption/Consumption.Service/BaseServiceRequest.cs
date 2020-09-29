/*
* 文件名    ：BaseServiceRequest<T>                             
* 程序说明  : 请求服务基类
* 更新时间  : 2020-05-21 16:47
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
        public async Task<Response> GetRequest<Response>(BaseRequest request, Method method) where Response : class
        {
            string pms = request.GetPropertiesObject();
            string url = requestUrl + request.route;
            if (!string.IsNullOrWhiteSpace(request.getParameter))
                url += request.getParameter;
            Response result = await restSharp.RequestBehavior<Response>(url, method, pms);
            return result;
        }

        /// <summary>
        /// 请求
        /// </summary>
        /// <typeparam name="Response"></typeparam>
        /// <param name="url">地址</param>
        /// <param name="pms">参数</param>
        /// <param name="method">方法类型</param>
        /// <returns></returns>
        public async Task<Response> GetRequest<Response>(string route, object obj, Method method) where Response : class
        {
            string pms = string.Empty;
            if (!string.IsNullOrWhiteSpace(obj?.ToString())) pms = JsonConvert.SerializeObject(obj);
            Response result = await restSharp.RequestBehavior<Response>(requestUrl + route, method, pms);
            return result;
        }
    }
}
