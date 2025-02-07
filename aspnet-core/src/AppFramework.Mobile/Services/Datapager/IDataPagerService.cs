using Abp.Application.Services.Dto;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AppFramework.Shared.Services.Datapager
{
    public interface IDataPagerService
    { 
        int SkipCount { get; set; }

        /// <summary>
        /// 选中项
        /// </summary>
        object SelectedItem { get; set; }

        /// <summary>
        /// 数据源
        /// </summary>
        ObservableCollection<object> GridModelList { get; set; }

        /// <summary>
        /// 设置数据源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        Task SetList<T>(IPagedResult<T> pagedResult);

        /// <summary>
        /// 设置数据源(集合)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listResult"></param>
        /// <returns></returns>
        Task SetList<T>(IListResult<T> listResult);
    }
}
