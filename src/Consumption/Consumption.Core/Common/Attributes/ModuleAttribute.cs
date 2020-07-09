/*
*
* 文件名    ：ModuleAttribute                             
* 程序说明  : 模块特性,标记该特性表示属于应用模块的部分
* 更新时间  : 2020-06-01 21:50
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.Core.Attributes
{
    using Consumption.Core.Enums;
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// 模块特性, 标记该特性表示属于应用模块的部分
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ModuleAttribute : Attribute
    {
        public ModuleAttribute(ModuleType moduleType, string desc, string typeName, string nameSpace, string icon)
        {
            this.desc = desc;
            this.icon = icon;
            this.typeName = typeName;
            this.moduleType = moduleType;
            this.nameSpace = nameSpace;
        }

        private ModuleType moduleType;
        private string desc;
        private string nameSpace;
        private string icon;
        private string typeName;

        /// <summary>
        /// 模块类型
        /// </summary>
        public ModuleType ModuleType
        {
            get { return moduleType; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Desc
        {
            get { return desc; }
        }

        /// <summary>
        /// 命名空间
        /// </summary>
        public string NameSpace
        {
            get { return nameSpace; }
        }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon
        {
            get { return icon; }
        }

        /// <summary>
        /// 类型
        /// </summary>
        public string TypeName
        {
            get { return typeName; }
        }
    }
}
