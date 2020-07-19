/*
*
* 文件名    ：SkinCenter                             
* 程序说明  : 样式控制类 
* 更新时间  : 2020-06-01 22:12
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.PC.ViewCenter
{
    using Consumption.Core.Attributes;
    using Consumption.PC.Common;
    using Consumption.PC.View;
    using Consumption.ViewModel;
    using Consumption.ViewModel.Common;
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using MaterialDesignColors;
    using MaterialDesignThemes.Wpf;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Media;

    /// <summary>
    /// 样式控制类
    /// </summary>
    [Module("个性化")]
    public class SkinCenter : BaseCenter<SkinView, SkinViewModel>
    {

    }

    /// <summary>
    /// 系统样式设置
    /// </summary>
    public class SkinViewModel : ViewModelBase
    {
        private readonly static PaletteHelper _paletteHelper = new PaletteHelper();

        public string SelectPageTitle { get; } = "个性化设置";

        //可选颜色集合-分组
        public IEnumerable<ISwatch> Swatches { get; } = SwatchHelper.Swatches;

        //改变颜色
        public RelayCommand<object> ChangeHueCommand { get; } = new RelayCommand<object>((t)=> ChangeHue(t));

        //改变主题
        public RelayCommand<object> ToggleBaseCommand { get; } = new RelayCommand<object>(o => ApplyBase((bool)o));

        private static void ApplyBase(bool isDark)
        {
            ModifyTheme(theme => theme.SetBaseTheme(isDark ? Theme.Dark : Theme.Light));
        }
        
        private static void ModifyTheme(Action<ITheme> modificationAction)
        {
            PaletteHelper paletteHelper = new PaletteHelper();
            ITheme theme = paletteHelper.GetTheme();
            modificationAction?.Invoke(theme);
            paletteHelper.SetTheme(theme);
        }

        private static void ChangeHue(object obj)
        {
            var hue = (Color)obj;
            _paletteHelper.ChangePrimaryColor(hue);
        }
    }
}
