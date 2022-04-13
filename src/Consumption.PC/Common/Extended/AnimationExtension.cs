namespace Consumption.PC.Common.Extended
{
    using System;
    using System.Windows;
    using System.Windows.Media.Animation;

    /// <summary>
    /// 动画帮助类
    /// </summary>
    public static class AnimationExtension
    {
        /// <summary>
        /// 创建宽度改变动画
        /// </summary>
        /// <param name="element">元素</param>
        /// <param name="Form">起始值</param>
        /// <param name="To">结束值</param>
        /// <param name="span">间隔</param>
        public static void CreateWidthChangedAnimation(this UIElement element, double Form, double To, TimeSpan span)
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
