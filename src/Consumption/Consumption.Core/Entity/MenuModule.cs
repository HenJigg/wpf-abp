/*
*
* 文件名    ：MenuModuleGroup                             
* 程序说明  : 存储菜单模块及子功能模块
* 更新时间  : 2020-07-08 11：29
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

namespace Consumption.Core.Entity
{
    using GalaSoft.MvvmLight;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;


    /// <summary>
    /// 菜单模块组
    /// </summary>
    public partial class MenuModuleGroup : ViewModelBase
    {
        public string MenuCode { get; set; }

        public string MenuName { get; set; }

        private ObservableCollection<MenuModule> modules = new ObservableCollection<MenuModule>();

        public ObservableCollection<MenuModule> Modules
        {
            get { return modules; }
            set { modules = value; RaisePropertyChanged(); }
        }
    }

    /// <summary>
    /// 菜单模块
    /// </summary>
    public class MenuModule : ViewModelBase
    {
        public string Name { get; set; }

        private int _value;

        /// <summary>
        /// 权限值
        /// </summary>
        public int Value
        {
            get { return _value; }
            set { _value = value; RaisePropertyChanged(); }
        }

        private bool isChecked;

        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; RaisePropertyChanged(); }
        }
    }
}
