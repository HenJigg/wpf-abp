/*
*
* 文件名    ：PaletteHelperExtensions                             
* 程序说明  : MaterialDesign 主题扩展
* 更新时间  : 2020-07-19 00：25
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
    using MaterialDesignColors;
    using MaterialDesignColors.ColorManipulation;
    using MaterialDesignThemes.Wpf;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Media;

    /// <summary>
    /// MaterialDesign 主题扩展
    /// </summary>
    public static class PaletteHelperExtensions
    {
        public static void ChangePrimaryColor(this PaletteHelper paletteHelper, Color color)
        {
            ITheme theme = paletteHelper.GetTheme();

            theme.PrimaryLight = new ColorPair(color.Lighten(), theme.PrimaryLight.ForegroundColor);
            theme.PrimaryMid = new ColorPair(color, theme.PrimaryMid.ForegroundColor);
            theme.PrimaryDark = new ColorPair(color.Darken(), theme.PrimaryDark.ForegroundColor);

            paletteHelper.SetTheme(theme);
        }

        public static void ChangeSecondaryColor(this PaletteHelper paletteHelper, Color color)
        {
            ITheme theme = paletteHelper.GetTheme();

            theme.SecondaryLight = new ColorPair(color.Lighten(), theme.SecondaryLight.ForegroundColor);
            theme.SecondaryMid = new ColorPair(color, theme.SecondaryMid.ForegroundColor);
            theme.SecondaryDark = new ColorPair(color.Darken(), theme.SecondaryDark.ForegroundColor);

            paletteHelper.SetTheme(theme);
        }
    }
}
