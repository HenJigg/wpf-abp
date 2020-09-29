/*
*
* 文件名    ：ModuleComponent                             
* 程序说明  : 模块组件, 用于加载程序集下的模块特性信息
* 更新时间  : 2020-05-12 21:50
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
    using Consumption.Shared.Common;
    using Consumption.Shared.Common.Attributes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;

    /// <summary>
    /// 模块组件
    /// </summary>
    public class ModuleComponent
    {
        /// <summary>
        /// 获取程序集下的所有具备模块特性的集合
        /// </summary>
        /// <returns>模块特性集合</returns>
        public async Task<List<ModuleAttribute>> GetAssemblyModules()
        {
            try
            {
                List<ModuleAttribute> list = new List<ModuleAttribute>();
                await Task.Run(() =>
                {
                    Assembly asm = Assembly.GetEntryAssembly();
                    var types = asm.GetTypes();
                    foreach (var t in types)
                    {
                        var attr = (ModuleAttribute)t.GetCustomAttribute(typeof(ModuleAttribute), false);
                        if (attr != null)
                            list.Add(attr);
                    }
                });
                return list;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 模块验证
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public bool ModuleVerify(ModuleAttribute module)
        {
            if (Contract.IsAdmin)
                return true;
            else
            {
                if (Contract.Menus
                    .FirstOrDefault(t => t.MenuName.Equals(module.Name)) != null)
                    return true;
            }
            return false;
        }
    }
}
