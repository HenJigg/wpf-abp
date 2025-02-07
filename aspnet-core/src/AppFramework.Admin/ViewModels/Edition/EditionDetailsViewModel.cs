using Abp.Application.Services.Dto;
using AppFramework.Shared;
using AppFramework.Admin.Models;
using AppFramework.Editions;
using AppFramework.Editions.Dto;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AppFramework.Common;
using Prism.Mvvm;

namespace AppFramework.Admin.ViewModels
{
    public class EditionDetailsViewModel : HostDialogViewModel
    {
        #region 字段/属性

        private readonly IEditionAppService appService;
        private readonly ICommonLookupAppService commonLookupAppService;

        private bool isPaid, isTrialActive, isWaitingDayAfter, isAssignToAnotherEdition;
        private EditionCreateModel model;
        private ObservableCollection<FlatFeatureModel> features;

        //是否付费
        public bool IsPaid
        {
            get { return isPaid; }
            set { isPaid = value; OnPropertyChanged(); }
        }

        //是否试用
        public bool IsTrialActive
        {
            get { return isTrialActive; }
            set { isTrialActive = value; OnPropertyChanged(); }
        }

        //订阅到期后
        public bool IsWaitingDayAfter
        {
            get { return isWaitingDayAfter; }
            set { isWaitingDayAfter = value; OnPropertyChanged(); }
        }

        //订阅到期后是否指定版本
        public bool IsAssignToAnotherEdition
        {
            get { return isAssignToAnotherEdition; }
            set { isAssignToAnotherEdition = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 当前新增或编辑的版本信息
        /// </summary>
        public EditionCreateModel Model
        {
            get { return model; }
            set { model = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 功能列表
        /// </summary>
        public ObservableCollection<FlatFeatureModel> Features
        {
            get { return features; }
            set { features = value; OnPropertyChanged(); }
        }

        #endregion 字段/属性

        public EditionDetailsViewModel(
            IEditionAppService appService,
            ICommonLookupAppService commonLookupAppService)
        {
            this.appService = appService;
            this.commonLookupAppService = commonLookupAppService;

            Model = new EditionCreateModel();
            Features = new ObservableCollection<FlatFeatureModel>();
            Editions = new ObservableCollection<SubscribableEditionComboboxItemDto>();
        }

        public override async Task Save()
        {
            //刷新界面设置的选项内容值
            RefreshInputInformation();

            if (!Verify(Model).IsValid) return;

            await SetBusyAsync(async () =>
            {
                List<NameValueDto> featureValues = new List<NameValueDto>();
                GetSelectedNodes(Features, ref featureValues);

                CreateOrUpdateEditionDto editionDto = new CreateOrUpdateEditionDto();
                editionDto.Edition = Map<EditionCreateDto>(Model);
                editionDto.FeatureValues = featureValues;

                await appService.CreateOrUpdate(editionDto).WebAsync(base.Save);
            });
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await SetBusyAsync(async () =>
            {
                int? id = null;
                if (parameters.ContainsKey("Value"))
                    id = parameters.GetValue<EditionListDto>("Value").Id;

                await appService.GetEditionForEdit(new NullableIdDto(id)).WebAsync(async result =>
                {
                    Model = Map<EditionCreateModel>(result.Edition);
                    var flats = Map<List<FlatFeatureModel>>(result.Features);
                    Features = CreateFeatureTrees(flats, null);
                    UpdateSelectedNodes(Features, result.FeatureValues);
                    await PopulateEditionsCombobox(() => { SetSelectedEdition(Model.Id); });
                });
            });
        }

        private void RefreshInputInformation()
        {
            //指定免费版时
            if (!IsPaid)
            {
                Model.DailyPrice = null;
                Model.WeeklyPrice = null;
                Model.MonthlyPrice = null;
                Model.AnnualPrice = null;
            }

            //如果试用到期
            if (!IsTrialActive)
                Model.TrialDayCount = null;

            //订阅到期后不设置等待期
            if (!IsWaitingDayAfter)
                Model.WaitingDayAfterExpire = null;

            //订阅到期后不切换指定版本
            if (!IsAssignToAnotherEdition)
                Model.ExpiringEditionId = null;
        }

        #region 租户选项

        private const string NotAssignedValue = "0";

        private SubscribableEditionComboboxItemDto? selectedEdition;

        /// <summary>
        /// 选中版本
        /// </summary>
        public SubscribableEditionComboboxItemDto? SelectedEdition
        {
            get => selectedEdition;
            set
            {
                selectedEdition = value;
                if (selectedEdition != null)
                {
                    //更新选中版本项刷新绑定的版本ID
                    if (int.TryParse(selectedEdition.Value, out int result))
                    {
                        model.ExpiringEditionId = result;
                    }
                }
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SubscribableEditionComboboxItemDto> editions;

        /// <summary>
        /// 版本列表
        /// </summary>
        public ObservableCollection<SubscribableEditionComboboxItemDto> Editions
        {
            get => editions;
            set
            {
                editions = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 获取及绑定可选的版本列表
        /// </summary>
        /// <param name="editionsPopulated"></param>
        /// <returns></returns>
        private async Task PopulateEditionsCombobox(Action editionsPopulated)
        {
            await commonLookupAppService.GetEditionsForCombobox().WebAsync(async result =>
             {
                 Editions.Clear();
                 AddNotAssignedItem();
                 foreach (var item in result.Items)
                     Editions.Add(item);
                 await Task.CompletedTask;
             });

            editionsPopulated();
        }

        private void AddNotAssignedItem()
        {
            Editions.Add(new SubscribableEditionComboboxItemDto(NotAssignedValue,
                string.Format("- {0} -", Local.Localize(AppLocalizationKeys.NotAssigned)), null));
        }

        private void SetSelectedEdition(int? editionId)
        {
            SelectedEdition = editionId.HasValue ?
                Editions.FirstOrDefault(e => e.Value == editionId.Value.ToString()) :
                Editions.FirstOrDefault(e => e.Value == NotAssignedValue);
        }

        #endregion 租户选项

        #region 功能列表

        /// <summary>
        /// 创建功能结点目录树
        /// </summary>
        /// <param name="flats"></param>
        /// <param name="parentName"></param>
        /// <returns></returns>
        private ObservableCollection<FlatFeatureModel> CreateFeatureTrees(List<FlatFeatureModel> flatFeatureModels, string parentName)
        {
            var trees = new ObservableCollection<FlatFeatureModel>();
            var nodes = flatFeatureModels.Where(q => q.ParentName == parentName).ToArray();

            foreach (var node in nodes)
            {
                node.Items = CreateFeatureTrees(flatFeatureModels, node.Name);
                trees.Add(node);
            }
            return trees;
        }

        /// <summary>
        /// 获取选中的功能节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="GrantedPermissionNames"></param>
        private void GetSelectedNodes(ObservableCollection<FlatFeatureModel> nodes, ref List<NameValueDto> featureValues)
        {
            foreach (var item in nodes)
            {
                if (bool.TryParse(item.DefaultValue, out bool result))
                    featureValues.Add(new NameValueDto(item.Name, item.IsChecked.ToString()));
                else
                    featureValues.Add(new NameValueDto(item.Name, item.DefaultValue));

                GetSelectedNodes(item.Items, ref featureValues);
            }
        }

        /// <summary>
        /// 更新选中功能节点
        /// </summary>
        /// <param name="GrantedPermissionNames"></param>
        private void UpdateSelectedNodes(ObservableCollection<FlatFeatureModel> flatFeatureModels, List<NameValueDto> nameValues)
        {
            nameValues.ForEach(item =>
            {
                UpdateSelected(flatFeatureModels, item);
            });
        }

        /// <summary>
        /// 设置选中功能节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="nodeName"></param>
        private void UpdateSelected(ObservableCollection<FlatFeatureModel> flats, NameValueDto item)
        {
            foreach (var flat in flats)
            {
                if (flat.Name.Equals(item.Name))
                {
                    if (bool.TryParse(item.Value, out bool result))
                    {
                        flat.IsChecked = result;
                        return;
                    }
                    else
                        flat.IsChecked = true;
                }

                UpdateSelected(flat.Items, item);
            }
        }

        #endregion 功能列表
    }
}
