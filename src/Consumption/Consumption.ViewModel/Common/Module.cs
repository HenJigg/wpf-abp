/*
*
* 文件名    ：Module                             
* 程序说明  : 定义模块的数据结构
* 更新时间  : 2020-05-12 21：09
* 
* 
*
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
        private int? auth;

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
        /// 权限值
        /// </summary>
        public int? Auth
        {
            get { return auth; }
            set { auth = value; RaisePropertyChanged(); }
        }
    }
}
