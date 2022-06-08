using AppFramework.ApiClient;
using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Localization;
using AppFramework.Localization.Dto;
using Prism.Commands;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class LanguageTextViewModel : NavigationCurdViewModel
    {
        public LanguageTextViewModel(ILanguageAppService appService, IApplicationContext context)
        {
            sources = new ObservableCollection<string>();
            baseLanguages = new ObservableCollection<LanguageStruct>();
            targetLanguages = new ObservableCollection<LanguageStruct>();

            input = new GetLanguageTextsInput()
            {
                FilterText = "",
                MaxResultCount = AppConsts.DefaultPageSize,
                SkipCount = 0
            };
            this.appService = appService;
            this.context = context;
            EditCommand = new DelegateCommand<LanguageTextListDto>(Edit);
            SearchCommand = new DelegateCommand(Search);

            dataPager.OnPageIndexChangedEventhandler += LanguageOnPageIndexChangedEventhandler;
        }

        #region 字段/属性

        private readonly ILanguageAppService appService;
        private readonly IApplicationContext context;

        public DelegateCommand SearchCommand { get; private set; }
        public DelegateCommand<LanguageTextListDto> EditCommand { get; private set; }

        private int targetIndex;
        private GetLanguageTextsInput input;
        private ObservableCollection<string> sources;
        private ObservableCollection<LanguageStruct> baseLanguages;
        private ObservableCollection<LanguageStruct> targetLanguages;
        private LanguageStruct selectedBaseLanguage;
        private LanguageStruct selectedTargetLanguage;

        public string Filter
        {
            get { return input.FilterText; }
            set { input.FilterText = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 目标值选项  全部/单个值
        /// </summary>
        public int TargetIndex
        {
            get { return targetIndex; }
            set
            {
                targetIndex = value;
                input.TargetValueFilter = value == 0 ? "ALL" : "EMPTY";
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<LanguageStruct> BaseLanguages
        {
            get { return baseLanguages; }
            set { baseLanguages = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<LanguageStruct> TargetLanguages
        {
            get { return targetLanguages; }
            set { targetLanguages = value; RaisePropertyChanged(); }
        }

        public ObservableCollection<string> Sources
        {
            get { return sources; }
            set { sources = value; RaisePropertyChanged(); }
        }

        public LanguageStruct SelectedBaseLanguage
        {
            get { return selectedBaseLanguage; }
            set
            {
                selectedBaseLanguage = value;
                input.BaseLanguageName = value.Name;
                RaisePropertyChanged();
            }
        }

        public LanguageStruct SelectedTargetLanguage
        {
            get { return selectedTargetLanguage; }
            set
            {
                selectedTargetLanguage = value;
                input.TargetLanguageName = value.Name;
                RaisePropertyChanged();
            }
        }

        public string SelectedSource
        {
            get { return input.SourceName; }
            set
            {
                input.SourceName = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        private async void LanguageOnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {
            input.SkipCount = e.SkipCount;
            input.MaxResultCount = e.PageSize;

            await SetBusyAsync(async () =>
             {
                 await GetLanguageTexts(input);
             });
        }

        private async void Edit(LanguageTextListDto obj)
        {
            DialogParameters param = new DialogParameters();
            param.Add("Value", obj);
            param.Add("Base", SelectedBaseLanguage);
            param.Add("Target", SelectedTargetLanguage);
            param.Add("SourceName", input.SourceName);

            var dialogResult = await dialog.ShowDialogAsync(AppViewManager.LanguageTextDetails, param);
            if (dialogResult.Result == ButtonResult.OK)
            {
                dataPager.PageIndex = 0;
            }
        }

        private void Search()
        {
            dataPager.PageIndex = 0;
        }

        private async Task GetLanguageTexts(GetLanguageTextsInput filter)
        {
            await WebRequest.Execute(() => appService.GetLanguageTexts(filter),
                     async result =>
                     {
                         dataPager.SetList(result);
                         await Task.CompletedTask;
                     });
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () => await GetLanguageTexts(input));
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext)
        {
            foreach (var item in context.Configuration.Localization.Sources)
            {
                Sources.Add(item.Name);
            }

            foreach (var item in context.Configuration.Localization.Languages)
            {
                BaseLanguages.Add(new LanguageStruct(item.Icon, item.Name, item.DisplayName));
                TargetLanguages.Add(new LanguageStruct(item.Icon, item.Name, item.DisplayName));
            }

            if (navigationContext.Parameters.ContainsKey("Name"))
            {
                var Name = navigationContext.Parameters.GetValue<string>("Name");

                var lang = context.Configuration.Localization.Languages.FirstOrDefault(t => t.Name.Equals(Name));
                if (lang != null)
                {
                    SelectedBaseLanguage = new LanguageStruct(lang.Icon, lang.Name, lang.DisplayName);
                    SelectedTargetLanguage = new LanguageStruct(lang.Icon, lang.Name, lang.DisplayName);
                }

                input.TargetValueFilter = "ALL";
                input.SourceName = Sources.Last();
            }

            await RefreshAsync();
        }
    }
}
