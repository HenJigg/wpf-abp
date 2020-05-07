using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Consumption.Core.UI_Interface
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
