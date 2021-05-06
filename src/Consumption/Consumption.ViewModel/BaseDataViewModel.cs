
namespace Consumption.ViewModel
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Linq;
    using Consumption.Shared.Common;
    using Consumption.ViewModel.Interfaces;
    using Consumption.Shared.Common.Query;
    using Consumption.Shared.Dto;
    using Microsoft.Toolkit.Mvvm.ComponentModel;
    using Microsoft.Toolkit.Mvvm.Input;
    using Microsoft.Toolkit.Mvvm.Messaging;
    using Prism.Ioc;

    /// <summary>
    /// 通用基类(实现CRUD/数据分页..)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseRepository<TEntity> : ObservableObject where TEntity : BaseDto, new()
    {
        protected readonly IConsumptionRepository<TEntity> repository;
        protected readonly IContainerProvider containerProvider;

        public BaseRepository(IConsumptionRepository<TEntity> repository,
            IContainerProvider containerProvider)
        {
            this.repository = repository;
            this.containerProvider = containerProvider;
        }

        #region ICrud (增删改查接口)

        private int selectPageIndex;
        private string search;
        private TEntity gridModel;
        private ObservableCollection<TEntity> gridModelList;
        public TEntity GridModel
        {
            get { return gridModel; }
            set { gridModel = value; OnPropertyChanged(); }
        }
        public int SelectPageIndex
        {
            get { return selectPageIndex; }
            set { selectPageIndex = value; OnPropertyChanged(); }
        }
        public string Search
        {
            get { return search; }
            set { search = value; OnPropertyChanged(); }
        }
        public ObservableCollection<TEntity> GridModelList
        {
            get { return gridModelList; }
            set { gridModelList = value; OnPropertyChanged(); }
        }
        public AsyncRelayCommand QueryCommand { get { return new AsyncRelayCommand(Query); } }
        public AsyncRelayCommand<string> ExecuteCommand { get { return new AsyncRelayCommand<string>(arg => Execute(arg)); } }

        /// <summary>
        /// 查询
        /// </summary>
        public virtual async Task Query()
        {
            await GetPageData(this.PageIndex);
        }

        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="arg"></param>
        public virtual async Task Execute(string arg)
        {
            /*
             * 这里使用string来做弱类型处理,防止使用枚举,
             * 其他页面需要重新该方法
             */
            switch (arg)
            {
                case "添加": AddAsync(); break;
                case "修改": UpdateAsync(); break;
                case "删除": await DeleteAsync(); break;
                case "保存": await SaveAsync(); break;
                case "取消": Cancel(); break;
            }
        }

        public virtual void AddAsync()
        {
            this.CreateDeaultCommand();
            GridModel = new TEntity();
            SelectPageIndex = 1;
        }

        public virtual void Cancel()
        {
            InitPermissions(this.AuthValue);
            SelectPageIndex = 0;
        }

        public virtual async Task DeleteAsync()
        {
            if (GridModel != null)
            {
                //if (await Msg.Question("确认删除当前选中行数据?"))
                {
                    var r = await repository.DeleteAsync(GridModel.Id);
                    if (r.StatusCode == 200)
                        await GetPageData(0);
                    else
                        WeakReferenceMessenger.Default.Send(r.Message, "Snackbar");
                }
            }
        }

        public virtual async Task SaveAsync()
        {
            //Before you save, you need to verify the validity of the data.
            if (GridModel == null) return;
            await repository.SaveAsync(GridModel);
            InitPermissions(this.AuthValue);
            await GetPageData(0);
            SelectPageIndex = 0;
        }

        public virtual async void UpdateAsync()
        {
            if (GridModel == null) return;
            var baseResponse = await repository.GetAsync(GridModel.Id);
            if (baseResponse.StatusCode == 200)
            {
                GridModel = baseResponse.Result;
                this.CreateDeaultCommand();
                SelectPageIndex = 1;
            }
            else
                WeakReferenceMessenger.Default.Send("Get data exception!", "Snackbar");
        }

        #endregion

        #region IDataPager (数据分页)
        public AsyncRelayCommand GoHomePageCommand { get { return new AsyncRelayCommand(GoHomePage); } }
        public AsyncRelayCommand GoOnPageCommand { get { return new AsyncRelayCommand(GoOnPage); } }
        public AsyncRelayCommand GoNextPageCommand { get { return new AsyncRelayCommand(GoNextPage); } }
        public AsyncRelayCommand GoEndPageCommand { get { return new AsyncRelayCommand(GoEndPage); } }

        private int totalCount = 0;
        private int pageSize = 30;
        private int pageIndex = 0;
        private int pageCount = 0;

        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get { return totalCount; } set { totalCount = value; OnPropertyChanged(); } }

        /// <summary>
        /// 当前页大小
        /// </summary>
        public int PageSize { get { return pageSize; } set { pageSize = value; OnPropertyChanged(); } }

        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get { return pageIndex; } set { pageIndex = value; OnPropertyChanged(); } }

        /// <summary>
        /// 分页总数
        /// </summary>
        public int PageCount { get { return pageCount; } set { pageCount = value; OnPropertyChanged(); } }

        /// <summary>
        /// 首页
        /// </summary>
        public virtual async Task GoHomePage()
        {
            if (this.PageIndex == 0) return;
            PageIndex = 0;
            await GetPageData(PageIndex);
        }

        /// <summary>
        /// 上一页
        /// </summary>
        public virtual async Task GoOnPage()
        {
            if (this.PageIndex == 0) return;
            PageIndex--;
            await this.GetPageData(PageIndex);
        }

        /// <summary>
        /// 下一页
        /// </summary>
        public virtual async Task GoNextPage()
        {
            if (this.PageIndex == PageCount) return;
            PageIndex++;
            await this.GetPageData(PageIndex);
        }

        /// <summary>
        /// 尾页
        /// </summary>
        public virtual async Task GoEndPage()
        {
            this.PageIndex = PageCount;
            await GetPageData(PageCount);
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="pageIndex"></param>
        public virtual async Task GetPageData(int pageIndex)
        {
            var r = await repository.GetAllListAsync(new QueryParameters()
            {
                PageIndex = this.PageIndex,
                PageSize = this.PageSize,
                Search = this.Search
            });
            if (r.StatusCode == 200)
            {
                GridModelList = new ObservableCollection<TEntity>(r.Result.Items.ToList());
                TotalCount = r.Result.TotalCount;
                SetPageCount();
            }
        }

        /// <summary>
        /// 设置页数
        /// </summary>
        public virtual void SetPageCount()
        {
            PageCount = Convert.ToInt32(Math.Ceiling((double)TotalCount / (double)PageSize));
        }
        #endregion

        #region IAuthority (权限内容)

        /// <summary>
        /// 创建页面默认命令
        /// </summary>
        public void CreateDeaultCommand()
        {
            ToolBarCommandList.Clear();
            ToolBarCommandList.Add(new CommandStruct() { CommandName = "保存", CommandColor = "#0066FF", CommandKind = "ContentSave" });
            ToolBarCommandList.Add(new CommandStruct() { CommandName = "取消", CommandColor = "#FF6633", CommandKind = "Cancel" });
        }

        private ObservableCollection<CommandStruct> toolBarCommandList;
        public ObservableCollection<CommandStruct> ToolBarCommandList
        {
            get { return toolBarCommandList; }
            set { toolBarCommandList = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 页面权限值
        /// </summary>
        public int AuthValue { get; private set; }

        /// <summary>
        /// 初始化权限
        /// </summary>
        public void InitPermissions(int AuthValue)
        {
            this.AuthValue = AuthValue;
            ToolBarCommandList = new ObservableCollection<CommandStruct>();
            Contract.AuthItems.ForEach(arg =>
            {
                if ((AuthValue & arg.AuthValue) == arg.AuthValue)
                    ToolBarCommandList.Add(new CommandStruct()
                    {
                        CommandName = arg.AuthName,
                        CommandKind = arg.AuthKind,
                        CommandColor = arg.AuthColor
                    });
            });
        }
        #endregion
    }
}
