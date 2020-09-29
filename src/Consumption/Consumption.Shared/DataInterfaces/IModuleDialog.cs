/*
*
* 文件名    ：IModuleDialog                             
* 程序说明  : 弹出窗口的上下文绑定接口
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
    /// 弹出式窗口接口
    /// </summary>
    public interface IModuleDialog
    {
        /// <summary>
        /// 弹出窗口
        /// </summary>
        Task<bool> ShowDialog();

        /// <summary>
        /// 注册模块事件
        /// </summary>
        void SubscribeEvent();

        /// <summary>
        /// 訂閱消息
        /// </summary>
        void SubscribeMessenger();

        /// <summary>
        /// 取消订阅消息
        /// </summary>
        void UnsubscribeMessenger();

    }
}
