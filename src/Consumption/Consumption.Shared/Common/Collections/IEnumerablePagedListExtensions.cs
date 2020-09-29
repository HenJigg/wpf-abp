
namespace Consumption.Shared.Common.Collections
{

    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides some extension methods for <see cref="IEnumerable{T}"/> to provide paging capability.
    /// 提供一些扩展方法来提供分页功能
    /// </summary>
    public static class IEnumerablePagedListExtensions
    {
        /// <summary>
        /// Converts the specified source to <see cref="IPagedList{T}"/> by the specified <paramref name="pageIndex"/> and <paramref name="pageSize"/>.
        /// 通过起始页和页大小把数据转换成页集合
        /// </summary>
        /// <typeparam name="T">The type of the source.源的类型</typeparam>
        /// <param name="source">The source to paging.分页的源</param>
        /// <param name="pageIndex">The index of the page.起始页</param>
        /// <param name="pageSize">The size of the page.页大小</param>
        /// <param name="indexFrom">The start index value.开始索引值</param>
        /// <returns>An instance of the inherited from <see cref="IPagedList{T}"/> interface.接口继承的实例</returns>
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize, int indexFrom = 0) => new PagedList<T>(source, pageIndex, pageSize, indexFrom);

        /// <summary>
        /// Converts the specified source to <see cref="IPagedList{T}"/> by the specified <paramref name="converter"/>, <paramref name="pageIndex"/> and <paramref name="pageSize"/>
        /// 通过转换器，起始页和页大小把数据转换成页集合
        /// </summary>
        /// <typeparam name="TSource">The type of the source.源的类型</typeparam>
        /// <typeparam name="TResult">The type of the result.反馈的类型</typeparam>
        /// <param name="source">The source to convert.要转换的源</param>
        /// <param name="converter">The converter to change the <typeparamref name="TSource"/> to <typeparamref name="TResult"/>.转换器来改变源到反馈</param>
        /// <param name="pageIndex">The page index.起始页</param>
        /// <param name="pageSize">The page size.页大小</param>
        /// <param name="indexFrom">The start index value.开始索引值</param>
        /// <returns>An instance of the inherited from <see cref="IPagedList{T}"/> interface.接口继承的实例</returns>
        public static IPagedList<TResult> ToPagedList<TSource, TResult>(this IEnumerable<TSource> source, Func<IEnumerable<TSource>, IEnumerable<TResult>> converter, int pageIndex, int pageSize, int indexFrom = 0) => new PagedList<TSource, TResult>(source, converter, pageIndex, pageSize, indexFrom);
    }
}
