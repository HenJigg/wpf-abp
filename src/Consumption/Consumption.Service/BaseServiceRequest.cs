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
    using Consumption.Core.Request;
    using Newtonsoft.Json;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 请求服务基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseServiceRequest<T>
    {
        /// <summary>
        /// restSharp实例
        /// </summary>
        public RestSharpCertificateMethod restSharp = new RestSharpCertificateMethod();

        /// <summary>
        /// 获取T请求
        /// </summary>
        /// <param name="request"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public async Task<T> GetRequest(BaseRequest request, Method method)
        {
            string pms = request.GetPropertiesObject();
            string url = request.route;
            if (!string.IsNullOrWhiteSpace(request.getParameter))
                url = request.route + request.getParameter;
            string resultString = await restSharp.RequestBehavior(url, method, pms);
            T result = JsonConvert.DeserializeObject<T>(resultString);
            return result;
        }
    }
}
