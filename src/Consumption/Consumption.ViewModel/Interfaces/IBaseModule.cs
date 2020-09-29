/*
*
* 文件名    ：IModule                             
* 程序说明  : 程序模块的上下文操作接口
* 更新时间  : 2020-05-11
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

using System;
using System.Threading.Tasks;

namespace Consumption.ViewModel.Interfaces
{
    public interface IBaseModule
    {
        /// <summary>
        /// 关联默认数据上下文
        /// </summary>
        void BindDefaultModel();

        object GetView();

        /// <summary>
        /// 关联默认数据上下文(包含权限相关)
        /// </summary>
        Task BindDefaultModel(int AuthValue = 0);

        /// <summary>
        /// 关联表格列
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        void BindDataGridColumns();
    }
}
