/*
*
* 文件名    ：IDataOperation                             
* 程序说明  : 程序基础页基类数据操作接口
* 更新时间  : 2020-05-11
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/
namespace Consumption.Core.Interfaces
{
    /// <summary>
    /// 数据操作接口
    /// </summary>
    public interface IDataOperation
    {
        /// <summary>
        /// 新增
        /// </summary>
        void Add<TModel>(TModel model);

        /// <summary>
        /// 编辑
        /// </summary>
        void Edit<TModel>(TModel model);

        /// <summary>
        /// 删除
        /// </summary>
        void Del<TModel>(TModel model);

        /// <summary>
        /// 查询
        /// </summary>
        void Query();

        /// <summary>
        /// 重置
        /// </summary>
        void Reset();

        /// <summary>
        /// 保存
        /// </summary>
        void Save();

        /// <summary>
        /// 取消
        /// </summary>
        void Cancel();
    }
}
