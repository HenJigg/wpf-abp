/*
*
* 文件名    ：Module                             
* 程序说明  : 定义模块的数据结构
* 更新时间  : 2020-06-01 22：06
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/

using GalaSoft.MvvmLight;

namespace Consumption.ViewModel.Common
{
    /// <summary>
    /// 模块
    /// </summary>
    public class Module : ViewModelBase
    {
        private string code;
        private string name;
        private int auth;
        private string typeName;

        /// <summary>
        /// 模块图标代码
        /// </summary>
        public string Code
        {
            get { return code; }
            set { code = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 模块命名空间
        /// </summary>
        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 权限值
        /// </summary>
        public int Auth
        {
            get { return auth; }
            set { auth = value; RaisePropertyChanged(); }
        }
    }

    /// <summary>
    /// 模块UI组件
    /// </summary>
    public class ModuleUIComponent : Module
    {
        private object body;

        /// <summary>
        /// 页面内容
        /// </summary>
        public object Body
        {
            get { return body; }
            set { body = value; RaisePropertyChanged(); }
        }
    }
}
