/*
*
* 文件名    ：IModule                             
* 程序说明  : 程序模块的上下文操作接口
* 更新时间  : 2020-05-11
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/

namespace Consumption.Core.Interfaces
{
    /// <summary>
    /// 模块
    /// </summary>
    public interface IModule
    {
        /// <summary>
        /// 关联数据上下文
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="viewModel"></param>
        void BindViewModel<TViewModel>(TViewModel viewModel) where TViewModel : class, new();

        /// <summary>
        /// 关联默认数据上下文
        /// </summary>
        void BindDefaultModel();

        /// <summary>
        /// 获取主窗口
        /// </summary>
        /// <returns></returns>
        object GetView();
    }
}
