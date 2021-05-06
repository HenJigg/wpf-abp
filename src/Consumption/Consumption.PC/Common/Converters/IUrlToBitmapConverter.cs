
/*
*
* 文件名    ：IUrlToBitmapConverter                             
* 程序说明  : 地址转图片转换器
* 更新时间  : 2020-06-02 21：10
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.PC.Common.Converters
{
    using System.IO;
    using System.Text;
    using System.Windows.Data;
    using System.Windows.Media.Imaging;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Consumption.PC.Common.Helpers;

    /// <summary>
    /// 地址转图片转换器
    /// </summary>
    public class IUrlToBitmapConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                string fileurl = $"{AppDomain.CurrentDomain.BaseDirectory}Skin\\Kind\\{value.ToString()}";
                if (File.Exists(fileurl))
                {
                    BitmapImage fileImg = ImageHelper.ConvertToImage(fileurl);
                    return fileImg;
                }
            }
            BitmapImage img = ImageHelper.ConvertToImage($"{AppDomain.CurrentDomain.BaseDirectory}Images\\background.png");
            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
