using Abp.Application.Services.Dto;
using AppFramework.Shared.Services.Mapper;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AppFramework.Shared.Services
{
    /// <summary>
    /// 数据分页服务
    /// </summary>
    public class DataPagerService : BindableBase, IDataPagerService
    {
        private readonly IAppMapper mapper;

        public DataPagerService(IAppMapper mapper)
        {
            pageSize = AppConsts.DefaultPageSize;
            numericButtonCount = 10;
            gridModelList = new ObservableCollection<object>();
            this.mapper = mapper;
        }

        private object selectedItem;
        private int pageIndex, pageCount, pageSize, numericButtonCount;
        private ObservableCollection<object> gridModelList;
        public event PageIndexChangedEventhandler OnPageIndexChangedEventhandler;

        public int NumericButtonCount
        {
            get { return numericButtonCount; }
            set
            {
                numericButtonCount = value;
                RaisePropertyChanged();
            }
        }

        public int PageIndex
        {
            get { return pageIndex; }
            set
            { 
                //分页组件的索引被UI当中变更,触发查询事件, 记录当前需要跳过的总数,以及当前的索引变化
                OnPageIndexChangedEventhandler?.Invoke(this, new PageIndexChangedEventArgs()
                {
                    OldPageIndex = pageIndex,
                    NewPageIndex = value,
                    SkipCount = value * PageSize,
                    PageSize = PageSize,
                });

                pageIndex = value;
                RaisePropertyChanged();
            }
        }

        public int PageCount
        {
            get { return pageCount; }
            set
            {
                pageCount = value;
                RaisePropertyChanged();
            }
        }

        public int PageSize
        {
            get { return pageSize; }
            set
            {
                if (pageSize == value) return;

                OnPageIndexChangedEventhandler?.Invoke(this, new PageIndexChangedEventArgs()
                {
                    OldPageIndex = pageIndex,
                    NewPageIndex = pageIndex,
                    SkipCount = pageIndex * PageSize,
                    PageSize = value,
                });
                pageSize = value;

                RaisePropertyChanged();
            }
        }

        public object SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<object> GridModelList
        {
            get { return gridModelList; }
            set { gridModelList = value; RaisePropertyChanged(); }
        }

        public async Task SetList<T>(IPagedResult<T> pagedResult)
        {
            GridModelList.Clear();

            foreach (var item in mapper.Map<List<T>>(pagedResult.Items))
                GridModelList.Add(item);

            if (pagedResult.TotalCount == 0)
                PageCount = 1;
            else
                PageCount = (int)Math.Ceiling(pagedResult.TotalCount / (double)PageSize);

            await Task.CompletedTask;
        }

        public async Task SetList<T>(IListResult<T> listResult)
        {
            await SetList<T>(new PagedResultDto<T>()
            {
                Items=listResult.Items
            });
        }
    }
}
