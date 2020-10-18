/*
*
* 文件名    ：BaseDataViewModel                             
* 程序说明  : 数据视图基类-GUID
* 更新时间  : 2020-07-11 17：53
* 联系作者  : QQ:779149549 
* 开发者群  : QQ群:874752819
* 邮件联系  : zhouhaogg789@outlook.com
* 视频教程  : https://space.bilibili.com/32497462
* 博客地址  : https://www.cnblogs.com/zh7791/
* 项目地址  : https://github.com/HenJigg/WPF-Xamarin-Blazor-Examples
* 项目说明  : 以上所有代码均属开源免费使用,禁止个人行为出售本项目源代码
* 
* 更新日期: 2020-09-11
* 更新内容: 重构基类的实现
*/


namespace Consumption.ViewModel
{
    using Consumption.ViewModel.Common;
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Linq;
    using Consumption.Shared.Common;
    using Consumption.ViewModel.Interfaces;
    using Consumption.Shared.DataModel;
    using Consumption.ViewModel.Common.Aop;
    using Consumption.Shared.Common.Query;
    using Consumption.Shared.Common.Aop;
    using Consumption.Shared.Dto;
    using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
    using Consumption.Shared.Common.Collections;
    using Newtonsoft.Json;
    using Microsoft.Toolkit.Mvvm.ComponentModel;
    using Microsoft.Toolkit.Mvvm.Input;
    using Microsoft.Toolkit.Mvvm.Messaging;

    /// <summary>
    /// 通用基类(实现CRUD/数据分页..)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseRepository<TEntity> : ObservableObject where TEntity : BaseDto, new()
    {
        public readonly IConsumptionRepository<TEntity> repository;

        public BaseRepository() { }

        public BaseRepository(IConsumptionRepository<TEntity> repository)
        {
            this.repository = repository;
        }

        #region ICrud (增删改查接口~喵)

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
        [GlobalLoger]
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
                if (await Msg.Question("确认删除当前选中行数据?"))
                {
                    var r = await repository.DeleteAsync(GridModel.Id);
                    if (r.StatusCode == 200)
                        await GetPageData(0);
                    else
                        WeakReferenceMessenger.Default.Send(r.Message, "Snackbar");
                }
            }
        }

        [GlobalProgress]
        public virtual async Task SaveAsync()
        {
            //Before you save, you need to verify the validity of the data.
            if (GridModel == null) return;
            await repository.SaveAsync(GridModel);
            InitPermissions(this.AuthValue);
            await GetPageData(0);
            SelectPageIndex = 0;
        }

        [GlobalProgress]
        public virtual async void UpdateAsync()
        {
            if (GridModel == null) return;
            var baseResponse = await repository.GetAsync(GridModel.Id);
            if (baseResponse.StatusCode == 200)
            {
                GridModel = JsonConvert.DeserializeObject<TEntity>(baseResponse.Result.ToString());
                this.CreateDeaultCommand();
                SelectPageIndex = 1;
            }
            else
                WeakReferenceMessenger.Default.Send("Get data exception!", "Snackbar");
        }

        #endregion

        #region IDataPager (数据分页~喵)
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
                var pagedList = JsonConvert.DeserializeObject<PagedList<TEntity>>(r.Result.ToString());
                GridModelList = new ObservableCollection<TEntity>(pagedList?.Items.ToList());
                TotalCount = GridModelList.Count;
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

        #region IAuthority (权限内容~)

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
