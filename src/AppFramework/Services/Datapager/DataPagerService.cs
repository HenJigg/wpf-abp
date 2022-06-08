using Abp.Application.Services.Dto;
using AutoMapper;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AppFramework.ViewModels
{
    /// <summary>
    /// 数据分页服务
    /// </summary>
    public class DataPagerService : BindableBase, IDataPagerService
    {
        private readonly IMapper mapper;

        public DataPagerService(IMapper mapper)
        {
            pageSize = AppConsts.DefaultPageSize;
            numericButtonCount = AppConsts.NumericButtonCount;
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
                if (value > 0 && pageIndex == value) return;

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
            set { pageCount = value; RaisePropertyChanged(); }
        }

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; RaisePropertyChanged(); }
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

        public void SetList<T>(IPagedResult<T> pagedResult)
        {
            GridModelList.Clear();

            foreach (var item in mapper.Map<List<T>>(pagedResult.Items))
                GridModelList.Add(item);

            if (pagedResult.TotalCount == 0)
                PageCount = 1;
            else
                PageCount = (int)Math.Ceiling(pagedResult.TotalCount / (double)PageSize);
        }
    }
}
