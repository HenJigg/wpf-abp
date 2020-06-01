/*
*
* 文件名    ：SkinViewModel                             
* 程序说明  : 系统样式设置
* 更新时间  : 2020-05-31 13:16
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.ViewModel
{
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// 系统样式设置
    /// </summary>
    public class SkinViewModel : BaseViewModel
    {
        public SkinViewModel()
        {
            ApplyCommand = new RelayCommand<string>(arg =>
              {
                  Messenger.Default.Send(arg, "UpdateBackground");
              });
        }

        /// <summary>
        /// 设置首页背景颜色
        /// </summary>
        public RelayCommand<string> ApplyCommand { get; private set; }
    }
}
