using Abp.Application.Services.Dto;
using Abp.Runtime.Security;
using Acr.UserDialogs;
using AppFramework.Shared.Core;
using AppFramework.Shared.Models;
using AppFramework.Editions.Dto;
using AppFramework.MultiTenancy;
using AppFramework.MultiTenancy.Dto;
using MvvmHelpers;
using Prism.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using AppFramework.Common;
using AppFramework.Shared.Services.Messenger;
using AppFramework.Shared.Extensions;

namespace AppFramework.Shared.ViewModels
{
    public class TenantDetailsViewModel : NavigationDetailViewModel
    {
        #region 字段/属性

        private readonly ITenantAppService tenantAppService;
        private readonly ICommonLookupAppService commonLookupAppService;
        private readonly IPermissionService permissionService;
        private readonly IMessenger messenger;

        private const string NotAssignedValue = "0";
        private bool isSubscriptionFieldVisible;
        private bool isUnlimitedTimeSubscription;
        private TenantListModel model;
        private ObservableRangeCollection<SubscribableEditionComboboxItemDto> editions;
        private SubscribableEditionComboboxItemDto selectedEdition;

        private bool useHostDatabase;
        private string adminPassword;
        private string adminPasswordRepeat;
        private bool isSetRandomPassword;

        public DateTime Today => DateTime.Now;

        public TenantListModel Model
        {
            get => model;
            set { model = value; RaisePropertyChanged(); }
        }

        public ObservableRangeCollection<SubscribableEditionComboboxItemDto> Editions
        {
            get => editions;
            set
            {
                editions = value;
                RaisePropertyChanged();
            }
        }

        //是否无限时间订阅
        public bool IsUnlimitedTimeSubscription
        {
            get => isUnlimitedTimeSubscription;
            set
            {
                isUnlimitedTimeSubscription = value;

                if (isUnlimitedTimeSubscription)
                {
                    //如果是无时间订阅限制, 将订阅事件设置为null
                    model.SubscriptionEndDateUtc = null;
                }
                else
                {
                    //如果不是无限时间订阅,重置是否到期选项
                    model.IsInTrialPeriod = false;
                }
                RaisePropertyChanged();
            }
        }

        //管理员密码
        public string AdminPassword
        {
            get => adminPassword;
            set
            {
                adminPassword = value;
                RaisePropertyChanged();
            }
        }

        //管理员密码重复
        public string AdminPasswordRepeat
        {
            get => adminPasswordRepeat;
            set
            {
                adminPasswordRepeat = value;
                RaisePropertyChanged();
            }
        }

        //使用主机数据库
        public bool UseHostDatabase
        {
            get => useHostDatabase;
            set
            {
                useHostDatabase = value;
                RaisePropertyChanged();
            }
        }

        //是否设置随机密码
        public bool IsSetRandomPassword
        {
            get => isSetRandomPassword;
            set
            {
                isSetRandomPassword = value;
                if (isSetRandomPassword)
                {
                    AdminPassword = null;
                    AdminPasswordRepeat = null;
                }
                RaisePropertyChanged();
            }
        }

        public bool IsSelectedEditionFree
        {
            get
            {
                if (Model == null)
                    return true;

                if (!Model.EditionId.HasValue)
                    return true;

                if (!SelectedEdition.IsFree.HasValue)
                    return true;

                return SelectedEdition.IsFree.Value;
            }
        }

        public SubscribableEditionComboboxItemDto SelectedEdition
        {
            get => selectedEdition;
            set
            {
                selectedEdition = value;
                UpdateModel();
                IsSubscriptionFieldVisible = SelectedEdition != null && SelectedEdition.Value != NotAssignedValue;
                RaisePropertyChanged("IsSelectedEditionFree");
                RaisePropertyChanged();
            }
        }

        //订阅字段是否可见
        public bool IsSubscriptionFieldVisible
        {
            get => isSubscriptionFieldVisible;
            set
            {
                isSubscriptionFieldVisible = value;
                RaisePropertyChanged();
            }
        }

        #endregion 字段/属性

        public TenantDetailsViewModel(ITenantAppService tenantAppService,
            ICommonLookupAppService commonLookupAppService,
            IPermissionService permissionService,
            IMessenger messenger)
        { 
            this.tenantAppService = tenantAppService;
            this.commonLookupAppService = commonLookupAppService;
            this.permissionService = permissionService;
            this.messenger = messenger;
            editions = new ObservableRangeCollection<SubscribableEditionComboboxItemDto>();
        }

        #region 保存/更新/删除/创建租户信息

        public override async void Save()
        {
            if (IsNewCeate)
            {
                var input = Map<CreateTenantInput>(Model);
                input.AdminPassword = AdminPassword;
                NormalizeTenantCreateInput(input);
                if (!Verify(input).IsValid) return;

                await CreateTenantAsync(input);
            }
            else
            {
                var input = Map<TenantEditDto>(Model);
                NormalizeTenantUpdateInput(input);
                if (!Verify(input).IsValid) return;

                await UpdateTenantAsync(input);
            }
        }

        private async Task UpdateTenantAsync(TenantEditDto input)
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() =>
                    tenantAppService.UpdateTenant(input),
                    GoBackAsync);
            }, LocalizationKeys.SavingWithThreeDot);
        }

        private async Task CreateTenantAsync(CreateTenantInput input)
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() =>
                    tenantAppService.CreateTenant(input),
                    GoBackAsync);
            }, LocalizationKeys.SavingWithThreeDot);
        }

        public override async void Delete()
        {
            var accepted = await UserDialogs.Instance.ConfirmAsync(
                Local.Localize(LocalizationKeys.TenantDeleteWarningMessage, Model.TenancyName),
                Local.Localize(LocalizationKeys.AreYouSure),
                Local.Localize(LocalizationKeys.Ok),
                Local.Localize(LocalizationKeys.Cancel));

            if (!accepted) return;

            await SetBusyAsync(async () =>
            {
                await tenantAppService.DeleteTenant(new EntityDto(Model.Id));
                await GoBackAsync();
            });
        }

        public override async Task GoBackAsync()
        {
            messenger.Send(AppMessengerKeys.Tenant);//通知更新列表
            await base.GoBackAsync();
        }

        #endregion 保存/更新/删除/创建租户信息

        public override async void OnNavigatedTo(Prism.Navigation.INavigationParameters parameters)
        {
            await SetBusyAsync(async () =>
            {
                if (parameters.ContainsKey("Value"))
                {
                    var tenant = parameters.GetValue<TenantListDto>("Value");
                    Model = Map<TenantListModel>(tenant);
                    InitializeEditTenant();
                }
                else
                    InitializeNewTenant();

                await PopulateEditionsCombobox(() =>
                {
                    if (Model != null)
                        SetSelectedEdition(Model.EditionId);
                });
            });
        }

        #region 内部方法

        private void InitializeNewTenant()
        {
            IsNewCeate = true;
            Model = new TenantListModel { IsActive = true };

            UseHostDatabase = true;
            IsSetRandomPassword = true;
        }

        private void InitializeEditTenant()
        {
            IsNewCeate = false;
            IsSetRandomPassword = false;
            UseHostDatabase = string.IsNullOrEmpty(Model.ConnectionString);
            if (!string.IsNullOrEmpty(Model.ConnectionString))
                Model.ConnectionString = TryDecryptConnectionString();
        }

        private string TryDecryptConnectionString()
        {
            try
            {
                return SimpleStringCipher.Instance.Decrypt(Model.ConnectionString);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void NormalizeTenantUpdateInput(TenantEditDto input)
        {
            input.EditionId = NormalizeEditionId(input.EditionId);
            input.SubscriptionEndDateUtc = NormalizeSubscriptionEndDateUtc(input.SubscriptionEndDateUtc);
        }

        private void NormalizeTenantCreateInput(CreateTenantInput input)
        {
            input.EditionId = NormalizeEditionId(input.EditionId);
            input.SubscriptionEndDateUtc = NormalizeSubscriptionEndDateUtc(input.SubscriptionEndDateUtc);
        }

        private int? NormalizeEditionId(int? editionId)
        {
            return editionId.HasValue && editionId.Value == 0 ? null : editionId;
        }

        private DateTime? NormalizeSubscriptionEndDateUtc(DateTime? subscriptionEndDateUtc)
        {
            if (IsUnlimitedTimeSubscription)
                return null;

            return subscriptionEndDateUtc.GetEndOfDate();
        }

        private async Task PopulateEditionsCombobox(Action editionsPopulated)
        {
            var editions = await commonLookupAppService.GetEditionsForCombobox();
            Editions.ReplaceRange(editions.Items);
            AddNotAssignedItem();
            editionsPopulated();
        }

        private void AddNotAssignedItem()
        {
            Editions.Insert(0, new SubscribableEditionComboboxItemDto(NotAssignedValue,
                string.Format("- {0} -", Local.Localize(LocalizationKeys.NotAssigned)), null));
        }

        private void SetSelectedEdition(int? editionId)
        {
            SelectedEdition = editionId.HasValue ?
                Editions.Single(e => e.Value == editionId.Value.ToString()) :
                Editions.Single(e => e.Value == NotAssignedValue);
        }

        private void UpdateModel()
        {
            if (SelectedEdition != null &&
                int.TryParse(SelectedEdition.Value, out var selectedEditionId))
                Model.EditionId = selectedEditionId;
            else
                Model.EditionId = null;

            Model.IsInTrialPeriod = !IsSelectedEditionFree;
        }

        #endregion 内部方法
    }
}