/*
*
* 文件名    ：IDataPager                             
* 程序说明  : 程序基础页的数据分页接口
* 更新时间  : 2020-05-11
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/


namespace Consumption.Shared.DataInterfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// 数据分页接口
    /// </summary>
    public interface IDataPager
    {
        /// <summary>
        /// 总行数
        /// </summary>
        int TotalCount { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// 当前页
        /// </summary>
        int PageIndex { get; set; }

        /// <summary>
        /// 总行数
        /// </summary>
        int PageCount { get; set; }

        /// <summary>
        /// 首页
        /// </summary>
        void GoHomePage();

        /// <summary>
        /// 上一页
        /// </summary>
        void GoOnPage();

        /// <summary>
        /// 下一页
        /// </summary>
        void GoNextPage();

        /// <summary>
        /// 尾页
        /// </summary>
        void GoEndPage();

        /// <summary>
        /// 获取分页数据
        /// </summary>
        /// <param name="pageIndex"></param>
        Task GetPageData(int pageIndex);
    }
}
