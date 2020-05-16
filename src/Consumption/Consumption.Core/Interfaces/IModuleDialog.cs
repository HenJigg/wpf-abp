/*
*
* 文件名    ：IModuleDialog                             
* 程序说明  : 弹出窗口的上下文绑定接口
* 更新时间  : 2020-05-11
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/

namespace Consumption.Core.Interfaces
{
    using System.Threading.Tasks;

    /// <summary>
    /// 弹出式窗口接口
    /// </summary>
    public interface IModuleDialog
    {
        /// <summary>
        /// 关联默认上下文
        /// </summary>
        void BindDefaultViewModel();

        /// <summary>
        /// 关联数据上下文
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="viewModel"></param>
        void BindViewModel<TViewModel>(TViewModel viewModel);

        /// <summary>
        /// 注册窗口默认事件
        /// </summary>
        void RegisterDefaultEvent();

        /// <summary>
        /// 弹出窗口
        /// </summary>
        Task<bool> ShowDialog();

        /// <summary>
        /// 关闭窗口
        /// </summary>
        void Close();

        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <returns></returns>
        object GetDialog();
    }
}
