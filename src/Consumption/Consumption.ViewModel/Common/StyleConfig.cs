/*
*
* 文件名    ：StyleConfig                             
* 程序说明  : 系统个性化设置
* 更新时间  : 2020-06-02 19：33
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.ViewModel.Common
{
    using GalaSoft.MvvmLight;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// 系统样式配置 -DeskTop
    /// </summary>
    public class StyleConfig : ViewModelBase
    {
        private string url;
        private double trans;
        private double radius;

        /// <summary>
        /// 背景图片
        /// </summary>
        public string Url
        {
            get { return url; }
            set
            {
                url = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 透明度
        /// </summary>
        public double Trans
        {
            get { return trans; }
            set { trans = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 高斯效果
        /// </summary>
        public double Radius
        {
            get { return radius; }
            set { radius = value; RaisePropertyChanged(); }
        }
    }
}
