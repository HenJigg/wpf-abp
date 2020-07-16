/*
*
* 文件名    ：ConsumptionResponse                             
* 程序说明  : 请求返回定义类
* 更新时间  : 2020-05-22 14：08
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/


namespace Consumption.Core.Response
{
    /// <summary>
    /// 请求返回定义类
    /// </summary>
    public class ConsumptionResponse
    {
        /// <summary>
        /// 后台消息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// //返回状态
        /// </summary>
        public int statusCode { get; set; }

        /// <summary>
        /// //分页的时候指定每页显示多少条数据
        /// </summary>
        public int pageSize { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int totalPage { get; set; }

        /// <summary>
        /// 是否分页
        /// </summary>
        public string paging { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// 总共的记录数
        /// </summary>
        public int TotalRecord { get; set; }

        public dynamic dynamicObj { get; set; }
    }
}
