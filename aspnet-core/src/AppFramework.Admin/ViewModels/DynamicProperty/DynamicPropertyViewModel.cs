using Abp.Application.Services.Dto;
using AppFramework.Shared;
using AppFramework.Shared.Services.Permission;
using AppFramework.DynamicEntityProperties;
using AppFramework.DynamicEntityProperties.Dto;
using Prism.Services.Dialogs;
using System.Threading.Tasks;
using Prism.Ioc; 
using Prism.Regions;
using AppFramework.Shared.Services;
using CommunityToolkit.Mvvm.Input;

namespace AppFramework.Admin.ViewModels
{
    public partial class DynamicPropertyViewModel : NavigationCurdViewModel
    {
        public IDataPagerService entitydataPager { get; private set; }
        private readonly IDynamicPropertyAppService appService;
        private readonly IDynamicEntityPropertyAppService entityPropertyAppService;


        public DynamicPropertyViewModel(
            IDynamicPropertyAppService appService,
            IDynamicEntityPropertyAppService entityPropertyAppService)
        {
            Title = Local.Localize("DynamicPropertyManagement");
            this.appService = appService;
            this.entityPropertyAppService = entityPropertyAppService;
            entitydataPager = ContainerLocator.Container.Resolve<IDataPagerService>();
        }

        [RelayCommand]
        private async void AddEntityProperty()
        {
            var r = await dialog.ShowDialogAsync(AppViews.DynamicAddEntity);
            if (r.Result == ButtonResult.OK)
            {
                var EntityFullName = r.Parameters.GetValue<string>("Value");
                Detail(new GetAllEntitiesHasDynamicPropertyOutput()
                {
                    EntityFullName = EntityFullName,
                });
            }
        }

        /// <summary>
        /// 动态实体属性详情
        /// </summary>
        /// <param name="output"></param>
        [RelayCommand]
        private async void Detail(GetAllEntitiesHasDynamicPropertyOutput output)
        {
            DialogParameters param = new DialogParameters();
            param.Add("Name", output.EntityFullName);

            await dialog.ShowDialogAsync(AppViews.DynamicEntityDetails, param);
            await SetBusyAsync(GetAllEntitiesHasDynamicProperty);
        }

        /// <summary>
        /// 删除动态属性
        /// </summary>
        private async void Delete()
        {
            if (dataPager.SelectedItem is DynamicPropertyDto item)
            {
                if (await dialog.Question(Local.Localize("DeleteDynamicPropertyMessage", item.DisplayName)))
                {
                    await SetBusyAsync(async () =>
                    {
                        await appService.Delete(new EntityDto(item.Id)).WebAsync(async () => await OnNavigatedToAsync());
                    });
                }
            }
        }

        /// <summary>
        /// 编辑值
        /// </summary>
        private async void EditValues()
        {
            DialogParameters param = new DialogParameters();
            param.Add("Value", dataPager.SelectedItem);

            var dialogResult = await dialog.ShowDialogAsync(AppViews.DynamicEditValues, param);
        }

        /// <summary>
        /// 获取动态属性
        /// </summary>
        /// <returns></returns>
        private async Task GetDynamicPropertyAll()
        {
            await appService.GetAll().WebAsync(dataPager.SetList);
        }

        /// <summary>
        /// 获取动态实体属性
        /// </summary>
        /// <returns></returns>
        private async Task GetAllEntitiesHasDynamicProperty()
        {
            await entityPropertyAppService.GetAllEntitiesHasDynamicProperty().WebAsync(entitydataPager.SetList);
        }


        /// <summary>
        /// 刷新动态属性模块
        /// </summary>
        /// <returns></returns>
        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            await SetBusyAsync(async () =>
            {
                await GetDynamicPropertyAll();
                await GetAllEntitiesHasDynamicProperty();
            });
        }

        public override PermissionItem[] CreatePermissionItems()
        {
            return new PermissionItem[]
             {
                new PermissionItem(AppPermissions.LanguageEdit, Local.Localize("Change"),Edit),
                new PermissionItem(AppPermissions.LanguageDelete, Local.Localize("Delete"),Delete),
                new PermissionItem(AppPermissions.LanguageChangeTexts, Local.Localize("EditValues"),EditValues),
             };
        }
    }
}
