using Abp.Application.Services.Dto;
using AutoMapper;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AppFramework.Shared.Services.Datapager
{
    public class DataPagerService : BindableBase, IDataPagerService
    {
        private readonly IAppMapper mapper;

        public DataPagerService(IAppMapper mapper)
        {
            gridModelList = new ObservableCollection<object>();
            this.mapper = mapper;
        }

        private object selectedItem;
        private int skipCount;
        private ObservableCollection<object> gridModelList;
        public int SkipCount
        {
            get { return skipCount; }
            set { skipCount = value; RaisePropertyChanged(); }
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
