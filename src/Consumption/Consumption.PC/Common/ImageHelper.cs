/*
*
* 文件名    ：ImageHelper                             
* 程序说明  : 图标操作类
* 更新时间  : 2020-05-32 15：15
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
    using System.IO;
    using System.Windows.Media.Imaging;
    using System.Drawing;
    using System.Collections.ObjectModel;
    using System.Windows.Interop;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 图标操作类
    /// </summary>
    public class ImageHelper
    {
        /// <summary>
        /// 获取背景预览图
        /// </summary>
        /// <returns></returns>
        public static async Task<ObservableCollection<SkinNode>> GetPrewView()
        {
            return await Task.Run(() =>
              {
                  string path = $"{AppDomain.CurrentDomain.BaseDirectory}Skin\\Preview";
                  if (Directory.Exists(path))
                  {
                      DirectoryInfo info = new DirectoryInfo(path);
                      var ifs = info.GetFiles()?.ToList();
                      ObservableCollection<SkinNode> skins = new ObservableCollection<SkinNode>();
                      ifs.ForEach(arg =>
                      {
                          skins.Add(new SkinNode()
                          {
                              Name = arg.Name,
                              Image = ImageHelper.ConvertToImage(arg.FullName)
                          });
                      });
                      return skins;
                  }
                  return null;
              });
        }

        public static BitmapImage ConvertToImage(string fileName)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.UriSource = new System.Uri(fileName);
            bmp.EndInit();
            bmp.Freeze();
            return bmp;
        }
    }
}
