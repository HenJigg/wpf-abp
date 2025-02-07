﻿using AppFramework.Shared;
using AppFramework.DynamicEntityProperties;
using AppFramework.DynamicEntityProperties.Dto;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using AppFramework.ApiClient;
using AppFramework.Admin.Services;
using System.Collections.ObjectModel;
using System.Linq;
using AppFramework.Shared.Services;

namespace AppFramework.Admin.ViewModels
{
    /// <summary>
    /// 动态实体属性详情
    /// </summary>
    public class DynamicEntityDetailsViewModel : HostDialogViewModel
    {
        private readonly IDynamicPropertyAppService propertyAppService;
        private readonly IDynamicEntityPropertyAppService appService;
        private readonly IApplicationContext context;
        public IDataPagerService dataPager { get; }
        private readonly IHostDialogService dialog;

        private DynamicPropertyDto selectedItem;
        private ObservableCollection<DynamicPropertyDto> items;

        private string EntityFullName;
        private bool isAdd;

        public bool IsAdd
        {
            get { return isAdd; }
            set { isAdd = value; OnPropertyChanged(); }
        }

        public DelegateCommand ShowAddCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; private set; }
        public DelegateCommand AddValueCommand { get; private set; }

        public DelegateCommand<DynamicEntityPropertyDto> DeleteCommand { get; private set; }

        public ObservableCollection<DynamicPropertyDto> Items
        {
            get { return items; }
            set { items = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 选中的实体属性
        /// </summary>
        public DynamicPropertyDto SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; OnPropertyChanged(); }
        }

        public DynamicEntityDetailsViewModel(
            IDynamicPropertyAppService propertyAppService,
            IDynamicEntityPropertyAppService appService,
            IApplicationContext context,
            IDataPagerService dataPager,
            IHostDialogService dialog)
        {
            this.dialog = dialog;
            this.context = context;
            this.dataPager = dataPager;
            this.appService = appService;
            this.propertyAppService = propertyAppService;

            items = new ObservableCollection<DynamicPropertyDto>();

            RefreshCommand = new DelegateCommand(Refresh);
            ShowAddCommand = new DelegateCommand(ShowAdd);
            AddValueCommand = new DelegateCommand(AddValue);
            DeleteCommand = new DelegateCommand<DynamicEntityPropertyDto>(Delete);
        }

        /// <summary>
        /// 添加实体属性
        /// </summary>
        private async void ShowAdd()
        {
            IsAdd = true;

            await SetBusyAsync(async () =>
            {
                await propertyAppService.GetAll().WebAsync(async result =>
                {
                    Items.Clear();
                    foreach (var item in result.Items)
                        Items.Add(item);

                    foreach (var item in dataPager.GridModelList)
                    {
                        var dynamicEntity = item as DynamicEntityPropertyDto;
                        if (dynamicEntity != null)
                        {
                            var t = Items.FirstOrDefault(t => t.Id.Equals(dynamicEntity.DynamicPropertyId));
                            if (t != null) Items.Remove(t);
                        }
                    }
                    await Task.CompletedTask;
                });
            });
        }

        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="obj"></param>
        private async void Delete(DynamicEntityPropertyDto obj)
        {
            if (await dialog.Question(Local.Localize("DeleteDynamicPropertyMessage"), AppSharedConsts.DialogIdentifier))
            {
                await SetBusyAsync(async () =>
                {
                    await appService.Delete(obj.Id).WebAsync(GetAllPropertiesOfAnEntity);
                });
            }
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        private async void Refresh()
        {
            await GetAllPropertiesOfAnEntity();
        }

        /// <summary>
        /// 添加值
        /// </summary>
        private async void AddValue()
        {
            if (SelectedItem == null) return;

            await SetBusyAsync(async () =>
            {
                await appService.Add(new DynamicEntityPropertyDto()
                {
                    TenantId = context.CurrentTenant?.TenantId,
                    EntityFullName = EntityFullName,
                    DynamicPropertyId = SelectedItem.Id
                })
                .WebAsync(GetAllPropertiesOfAnEntity);
            });
        }

        public async Task GetAllPropertiesOfAnEntity()
        {
            IsAdd = false;

            await SetBusyAsync(async () =>
            {
                await appService.GetAllPropertiesOfAnEntity(new DynamicEntityPropertyGetAllInput()
                {
                    EntityFullName = EntityFullName
                }).WebAsync(dataPager.SetList);
            });
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Name"))
            {
                EntityFullName = parameters.GetValue<string>("Name");

                await GetAllPropertiesOfAnEntity();
            }
        }
    }
}
