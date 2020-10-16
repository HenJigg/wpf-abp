/*
*
* 文件名    ：ICrud<T> T:BaseEntity                             
* 程序说明  : 基础增删改查接口
* 更新时间  : 2020-09-12 11:34
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
*/


namespace Consumption.ViewModel.Interfaces
{
    using Microsoft.Toolkit.Mvvm.Input;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    public interface ICrud<TEntity> where TEntity : class
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
