namespace Consumption.ViewModel.Common
{
    using Microsoft.Toolkit.Mvvm.Input;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    public interface IOrdinary<TEntity> where TEntity : class
    {
        /// <summary>
        /// 选中表单
        /// </summary>
        TEntity GridModel { get; set; }
        /// <summary>
        /// 页索引
        /// </summary>
        int SelectPageIndex { get; set; }

        /// <summary>
        /// 搜索参数
        /// </summary>
        string Search { get; set; }

        /// <summary>
        /// 表单
        /// </summary>
        ObservableCollection<TEntity> GridModelList { get; set; }

        /// <summary>
        /// 搜索命令
        /// </summary>
        AsyncRelayCommand QueryCommand { get; }

        /// <summary>
        /// 其它命令
        /// </summary>
        AsyncRelayCommand<string> ExecuteCommand { get; }

        /// <summary>
        /// 添加
        /// </summary>
        void AddAsync();

        /// <summary>
        /// 更新
        /// </summary>
        void UpdateAsync();

        /// <summary>
        /// 编辑
        /// </summary>
        Task DeleteAsync();

        /// <summary>
        /// 保存
        /// </summary>
        Task SaveAsync();

        /// <summary>
        /// 取消
        /// </summary>
        void Cancel();
    }
}
