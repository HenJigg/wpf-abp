using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Common.Services.Permission;
using AppFramework.DynamicEntityProperties;
using AppFramework.DynamicEntityProperties.Dto;
using Prism.Services.Dialogs;
using System.Threading.Tasks;
using Prism.Ioc;
using Prism.Commands;
using System;

namespace AppFramework.ViewModels
{
    public class DynamicPropertyViewModel : NavigationCurdViewModel
    {
        public IDataPagerService entitydataPager { get; private set; }
        private readonly IDynamicPropertyAppService appService;
        private readonly IDynamicEntityPropertyAppService entityPropertyAppService;

        public DelegateCommand AddEntityPropertyCommand { get; private set; }
        public DelegateCommand<GetAllEntitiesHasDynamicPropertyOutput> DetailCommand { get; private set; }

        public DynamicPropertyViewModel(
            IDynamicPropertyAppService appService,
            IDynamicEntityPropertyAppService entityPropertyAppService)
        {
            this.appService = appService;
            this.entityPropertyAppService = entityPropertyAppService;
            entitydataPager = ContainerLocator.Container.Resolve<IDataPagerService>();

            DetailCommand = new DelegateCommand<GetAllEntitiesHasDynamicPropertyOutput>(Show);
            AddEntityPropertyCommand = new DelegateCommand(AddEntityProperty);
        }

        private async void AddEntityProperty()
        {
            var r = await dialog.ShowDialogAsync(AppViewManager.DynamicAddEntity);
            if (r.Result == ButtonResult.OK)
            {
                var EntityFullName = r.Parameters.GetValue<string>("Value");
                Show(new GetAllEntitiesHasDynamicPropertyOutput()
                {
                    EntityFullName = EntityFullName,
                });
            }
        }

        /// <summary>
        /// 动态实体属性详情
        /// </summary>
        /// <param name="output"></param>
        private async void Show(GetAllEntitiesHasDynamicPropertyOutput output)
        {
            DialogParameters param = new DialogParameters();
            param.Add("Name", output.EntityFullName);

            await dialog.ShowDialogAsync(AppViewManager.DynamicEntityDetails, param);
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
                        await WebRequest.Execute(() => appService.Delete(
                            new EntityDto(item.Id)),
                            RefreshAsync);
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

            var dialogResult = await dialog.ShowDialogAsync(AppViewManager.DynamicEditValues, param);
        }

        /// <summary>
        /// 获取动态属性
        /// </summary>
        /// <returns></returns>
        private async Task GetDynamicPropertyAll()
        {
            await WebRequest.Execute(() => appService.GetAll(),
                       async result =>
                       {
                           dataPager.SetList(new PagedResultDto<DynamicPropertyDto>()
                           {
                               Items = result.Items
                           });
                           await Task.CompletedTask;
                       });
        }

        /// <summary>
        /// 获取动态实体属性
        /// </summary>
        /// <returns></returns>
        private async Task GetAllEntitiesHasDynamicProperty()
        {
            await WebRequest.Execute(() => entityPropertyAppService.GetAllEntitiesHasDynamicProperty(),
                       async result =>
                       {
                           entitydataPager.SetList(new PagedResultDto<GetAllEntitiesHasDynamicPropertyOutput>()
                           {
                               Items = result.Items
                           });
                           await Task.CompletedTask;
                       });
        }


        /// <summary>
        /// 刷新动态属性模块
        /// </summary>
        /// <returns></returns>
        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await GetDynamicPropertyAll();
                await GetAllEntitiesHasDynamicProperty();
            });
        }

        public override PermissionItem[] GetDefaultPermissionItems()
        {
            return new PermissionItem[]
             {
                new PermissionItem(Permkeys.LanguageEdit, Local.Localize("Change"),()=>Edit()),
                new PermissionItem(Permkeys.LanguageDelete, Local.Localize("Delete"),()=>Delete()),
                new PermissionItem(Permkeys.LanguageChangeTexts, Local.Localize("EditValues"),()=>EditValues()),
             };
        }
    }
}
