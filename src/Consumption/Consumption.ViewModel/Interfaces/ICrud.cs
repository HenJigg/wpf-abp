using Autofac.Extras.DynamicProxy;
using Consumption.Core.Aop;
using Consumption.Core.Query;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Consumption.ViewModel.Interfaces
{
    [Intercept(typeof(GlobalLoger))]
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
        RelayCommand QueryCommand { get; }

        /// <summary>
        /// 其它命令
        /// </summary>
        RelayCommand<string> ExecuteCommand { get; }

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
        void DeleteAsync();

        /// <summary>
        /// 保存
        /// </summary>
        void SaveAsync();

        /// <summary>
        /// 取消
        /// </summary>
        void Cancel();
    }
}
