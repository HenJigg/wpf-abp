/*
* 文件名称  : RestSharpCertificateMethod.cs                       
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
    using Consumption.Core.Aop;
    using RestSharp;
    using System.Threading.Tasks;

    /// <summary>
    /// RestSharp Client
    /// </summary>
    public class RestSharpCertificateMethod
    {
        /// <summary>
        /// 请求数据
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="method">请求类型</param>
        /// <param name="pms">参数</param>
        /// <param name="isToken">是否Token</param>
        /// <param name="isJson">是否Json</param>
        /// <returns></returns>
        [GlobalLoger]
        public async Task<string> RequestBehavior(string url, Method method, string pms,
            bool isToken = true, bool isJson = true)
        {
            RestClient client = new RestClient(url);
            RestRequest request = new RestRequest(method);
            if (isToken)
            {
                client.AddDefaultHeader("token", "");
            }
            switch (method)
            {
                case Method.GET:
                    request.AddHeader("Content-Type", "application/json");
                    break;
                case Method.POST:
                    if (isJson)
                    {
                        request.AddHeader("Content-Type", "application/json");
                        request.AddJsonBody(pms);
                    }
                    else
                    {
                        request.AddHeader("Content-Type", "application/json");
                        request.AddParameter("application/x-www-form-urlencoded",
                            pms, ParameterType.RequestBody);
                    }
                    break;
                case Method.PUT:
                    request.AddHeader("Content-Type", "application/json");
                    break;
                case Method.DELETE:
                    request.AddHeader("Content-Type", "application/json");
                    break;
                default:
                    request.AddHeader("Content-Type", "application/json");
                    break;
            }
            var response = await client.ExecuteAsync(request);
            return response.Content;
        }
    }
}
