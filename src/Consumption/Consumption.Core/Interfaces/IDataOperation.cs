/*
*
* 文件名    ：IDataOperation                             
* 程序说明  : 程序基础页基类数据操作接口
* 更新时间  : 2020-05-11
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
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
