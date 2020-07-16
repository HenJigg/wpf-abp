/*
*
* 文件名    ：AnimationHelper                             
* 程序说明  : 动画帮助类
* 更新时间  : 2020-07-11 00：18
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.PC.Common
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows;
    using System.Windows.Media.Animation;

    /// <summary>
    /// 动画帮助类
    /// </summary>
    public class AnimationHelper
    {
        /// <summary>
        /// 创建宽度改变动画
        /// </summary>
        /// <param name="element">元素</param>
        /// <param name="Form">起始值</param>
        /// <param name="To">结束值</param>
        /// <param name="span">间隔</param>
        public static void CreateWidthChangedAnimation(UIElement element, double Form, double To, TimeSpan span)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = Form;
            doubleAnimation.To = To;
            doubleAnimation.Duration = span;
            Storyboard.SetTarget(doubleAnimation, element);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("Width"));
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(doubleAnimation);
            storyboard.Begin();
        }

    }
}
