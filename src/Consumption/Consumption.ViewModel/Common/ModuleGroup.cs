/*
*
* 文件名    ：ModuleGroup                             
* 程序说明  : 定义分组模块的数据结构
* 更新时间  : 2020-05-12 21：09
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
    using Microsoft.Toolkit.Mvvm.ComponentModel;
    using System.Collections.ObjectModel;

    /// <summary>
    /// 模块分组
    /// </summary>
    public class ModuleGroup : ObservableObject
    {
        private string groupName;
        private bool contractionTemplate = true;
        private ObservableCollection<Module> modules;

        /// <summary>
        /// 组名称
        /// </summary>
        public string GroupName
        {
            get { return groupName; }
            set { groupName = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 收缩面板-模板
        /// </summary>
        public bool ContractionTemplate
        {
            get { return contractionTemplate; }
            set { contractionTemplate = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 包含的子模块
        /// </summary>
        public ObservableCollection<Module> Modules
        {
            get { return modules; }
            set { modules = value; OnPropertyChanged(); }
        }
    }
}
