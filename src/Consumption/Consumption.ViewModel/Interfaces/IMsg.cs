/*
*
* 文件名    ：IMsg                             
* 程序说明  : 消息提示接口
* 更新时间  : 2020-05-11
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

using System.Threading.Tasks;

namespace Consumption.ViewModel.Interfaces
{
    /// <summary>
    /// 消息接口
    /// </summary>
    public interface IMsg
    {
        /// <summary>
        /// 询问
        /// </summary>
        /// <param name="msg">内容</param>
        Task<bool> Show(object obj);
    }
}
