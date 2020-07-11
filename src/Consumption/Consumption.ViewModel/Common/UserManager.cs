/*
*
* 文件名    ：StyleConfig                             
* 程序说明  : 系统个性化设置
* 更新时间  : 2020-06-02 19：36
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
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// 系统个性化设置
    /// </summary>
    public class UserManager
    {
        private readonly static string styleConfigPath = AppDomain.CurrentDomain.BaseDirectory + "Config\\style.json";

        /// <summary>
        /// 获取个性化配置
        /// </summary>
        /// <returns></returns>
        public static StyleConfig GetStyleConfig()
        {
            try
            {
                if (File.Exists(styleConfigPath))
                {
                    string jsonStyle = File.ReadAllText(styleConfigPath);
                    var style = JsonConvert.DeserializeObject<StyleConfig>(jsonStyle);
                    return style;
                }
                return new StyleConfig() { Radius = 0, Trans = 0 };
            }
            catch
            {
                return new StyleConfig() { Radius = 0, Trans = 0 };
            }
        }

        /// <summary>
        /// 保存个性化配置
        /// </summary>
        /// <param name="config"></param>
        public static void SaveStyleConfig(StyleConfig config)
        {
            FileInfo fi = new FileInfo(styleConfigPath);
            var di = fi.Directory;
            if (!di.Exists)
                di.Create();
            string jsonStyle = JsonConvert.SerializeObject(config);
            File.WriteAllText(styleConfigPath, jsonStyle);
        }
    }
}
