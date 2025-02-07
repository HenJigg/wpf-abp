using Abp.Application.Services.Dto;
using AppFramework.Editions;
using AppFramework.Editions.Dto;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AppFramework.Shared.Models;
using AppFramework.Shared.Core;
using AppFramework.Common;
using AppFramework.Shared.Services.Messenger;
using AppFramework.Shared.Extensions;

namespace AppFramework.Shared.ViewModels
{
    public class EditionDetailsViewModel : NavigationDetailViewModel
    {
        #region 字段/属性

        private readonly IMessenger messenger;
        private readonly IEditionAppService appService;
        private readonly ICommonLookupAppService commonLookupAppService;

        private bool isPaid, isTrialActive, isWaitingDayAfter, isAssignToAnotherEdition;
        private EditionCreateModel model;
        private ObservableCollection<FlatFeatureModel> features;

        //是否付费
        public bool IsPaid
        {
            get { return isPaid; }
            set { isPaid = value; RaisePropertyChanged(); }
        }

        //是否试用
        public bool IsTrialActive
        {
            get { return isTrialActive; }
            set { isTrialActive = value; RaisePropertyChanged(); }
        }

        //订阅到期后
        public bool IsWaitingDayAfter
        {
            get { return isWaitingDayAfter; }
            set { isWaitingDayAfter = value; RaisePropertyChanged(); }
        }

        //订阅到期后是否指定版本
        public bool IsAssignToAnotherEdition
        {
            get { return isAssignToAnotherEdition; }
            set { isAssignToAnotherEdition = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 当前新增或编辑的版本信息
        /// </summary>
        public EditionCreateModel Model
        {
            get { return model; }
            set { model = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 功能列表
        /// </summary>
        public ObservableCollection<FlatFeatureModel> Features
        {
            get { return features; }
            set { features = value; RaisePropertyChanged(); }
        }
         
        #endregion 字段/属性

        public EditionDetailsViewModel(IMessenger messenger,
            IEditionAppService appService,
            ICommonLookupAppService commonLookupAppService)
        {
            this.messenger = messenger;
            this.appService = appService;
            this.commonLookupAppService = commonLookupAppService;

            Model = new EditionCreateModel();
            Features = new ObservableCollection<FlatFeatureModel>();
            Editions = new ObservableCollection<SubscribableEditionComboboxItemDto>();
        }

        /// <summary>
        /// 保存
        /// </summary>
        public override async void Save()
        {
            //刷新界面设置的选项内容值
            RefreshInputInformation();

            if (!Verify(Model).IsValid) return;

            await SetBusyAsync(async () =>
            { 
                await WebRequest.Execute(async () =>
                {
                    List<NameValueDto> featureValues = new List<NameValueDto>();
                    GetSelectedNodes(Features, ref featureValues);

                    CreateOrUpdateEditionDto editionDto = new CreateOrUpdateEditionDto();
                    editionDto.Edition=Map<EditionCreateDto>(Model);
                    editionDto.FeatureValues=featureValues;
                    await appService.CreateOrUpdate(editionDto);
                }, async () => await GoBackAsync());
            }, LocalizationKeys.SavingWithThreeDot);
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

        public override async Task GoBackAsync()
        {
            messenger.Send(AppMessengerKeys.Edition);
            await base.GoBackAsync();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            await SetBusyAsync(async () =>
            {
                int? id = null;
                if (parameters.ContainsKey("Value"))
                    id = parameters.GetValue<EditionListDto>("Value").Id;

                await WebRequest.Execute(() =>
                  appService.GetEditionForEdit(new NullableIdDto(id)),
                  async result =>
                  {
                      //设置编辑版本信息对应的内容
                      Model = Map<EditionCreateModel>(result.Edition);

                      //设置所包含对应的功能结点
                      var flats = Map<List<FlatFeatureModel>>(result.Features);
                      Features = CreateFeatureTrees(flats, null);

                      //更新选中的版本功能结点信息 
                      UpdateSelectedNodes(Features, result.FeatureValues);

                      await PopulateEditionsCombobox(() =>
                      {
                          SetSelectedEdition(Model.Id);
                      });
                  });
            });
        }

        #region 租户选项

        private const string NotAssignedValue = "0";

        private SubscribableEditionComboboxItemDto selectedEdition;

        /// <summary>
        /// 选中版本
        /// </summary>
        public SubscribableEditionComboboxItemDto SelectedEdition
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
                RaisePropertyChanged();
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
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 获取及绑定可选的版本列表
        /// </summary>
        /// <param name="editionsPopulated"></param>
        /// <returns></returns>
        private async Task PopulateEditionsCombobox(Action editionsPopulated)
        {
            var editions = await commonLookupAppService.GetEditionsForCombobox();

            Editions.Clear();

            AddNotAssignedItem();

            foreach (var item in editions.Items)
                Editions.Add(item);

            editionsPopulated();
        }

        private void AddNotAssignedItem()
        {
            Editions.Add(new SubscribableEditionComboboxItemDto(NotAssignedValue,
                string.Format("- {0} -", Local.Localize(LocalizationKeys.NotAssigned)), null));
        }

        private void SetSelectedEdition(int? editionId)
        {
            SelectedEdition = editionId.HasValue ?
                Editions.Single(e => e.Value == editionId.Value.ToString()) :
                Editions.Single(e => e.Value == NotAssignedValue);
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