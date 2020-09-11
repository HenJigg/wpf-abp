/*
*
* 文件名    ：BaseRequest                             
* 程序说明  : 请求参数构造基类
* 更新时间  : 2020-05-30 14：18
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.Core.Request
{
    using Consumption.Common.Contract;
    using Consumption.Core.Attributes;
    using Newtonsoft.Json;
    using System;
    using System.Reflection;
    using System.Text;
    using System.Web;

    /// <summary>
    ///  请求参数构造基类
    /// </summary>
    public class BaseRequest
    {
        /// <summary>
        /// 路由地址
        /// </summary>
        [Prevent]
        public virtual string route { get; set; }

        [Prevent]
        public string getParameter { get; set; }

        /// <summary>
        /// 解析对象参数
        /// </summary>
        /// <returns></returns>
        public string GetPropertiesObject()
        {
            StringBuilder getBuilder = new StringBuilder();
            StringBuilder builder = new StringBuilder();
            var type = this.GetType();
            var propertyArray = type.GetProperties();
            if (propertyArray != null && propertyArray.Length > 0)
            {
                foreach (PropertyInfo property in propertyArray)
                {
                    var prevent = property.GetCustomAttribute<PreventAttribute>();
                    if (prevent != null) continue;
                    var pvalue = property.GetValue(this);
                    if (pvalue == null) continue;
                    var str = pvalue.GetType().Namespace;
                    if (pvalue != null && pvalue.GetType().Namespace == "Consumption.Core.Query")
                    {
                        //当参数作为Query类型是, 则进行拆解对象拼接字符串
                        StringBuilder pbuilder = new StringBuilder();
                        var QpropertyArray = pvalue.GetType().GetProperties();
                        if (QpropertyArray != null && QpropertyArray.Length > 0)
                        {
                            foreach (PropertyInfo Qproperty in QpropertyArray)
                            {
                                var Qprevent = Qproperty.GetCustomAttribute<PreventAttribute>();
                                if (Qprevent != null)
                                    continue;
                                var Qpvalue = Qproperty.GetValue(pvalue);
                                if (Qpvalue != null && Qpvalue.ToString() != "")
                                {
                                    if (getBuilder.ToString() == string.Empty) getBuilder.Append("?");
                                    getBuilder.Append(Qproperty.Name + "=" +
                                        HttpUtility.UrlEncode(Convert.ToString(Qpvalue)) + "&");
                                }
                            }
                        }
                        getBuilder.Append(pbuilder.ToString());
                    }
                    else if (pvalue != null &&
                        (pvalue.GetType().Namespace == "Consumption.Core.Entity" ||
                        pvalue.GetType().Namespace == "Consumption.Core.RequestForm"))
                    {
                        //当属性为对象得情况下, 进行序列化
                        pvalue = JsonConvert.SerializeObject(pvalue);
                        builder.Append(pvalue);
                    }
                    else
                    {
                        //当属性为C#基础类型得情况下,默认Get传参, 拼接至路由地址中
                        if (getBuilder.ToString() == string.Empty) getBuilder.Append("?");
                        getBuilder.Append($"{property.Name}={HttpUtility.UrlEncode(Convert.ToString(pvalue))}&");
                    }
                }
            }

            string getStr = getBuilder.ToString().Trim('&');
            if (!string.IsNullOrWhiteSpace(getStr))
                getParameter = getStr;
            return builder.ToString().Trim('&');
        }

        /// <summary>
        /// 获取请求API地址
        /// </summary>
        /// <param name="addr"></param>
        /// <returns></returns>
        public string GetRouteUrl(string addr)
        {
            return route += addr;
        }
    }
}
