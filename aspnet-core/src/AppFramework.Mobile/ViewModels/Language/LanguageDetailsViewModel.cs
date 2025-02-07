using AppFramework.ApiClient;
using AppFramework.Localization;
using AppFramework.Localization.Dto;
using AppFramework.Shared.Models;
using AppFramework.Shared.Services.Datapager;
using Prism.Navigation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AppFramework.Shared.ViewModels
{
    public class LanguageDetailsViewModel : NavigationViewModel
    {
        private GetLanguageTextsInput input;
        private readonly IApplicationContext context;
        private readonly ILanguageAppService appService;
        public IDataPagerService dataPager { get; private set; }

        public LanguageDetailsViewModel(ILanguageAppService appService, IDataPagerService dataPager, IApplicationContext context)
        {
            this.context=context;
            this.dataPager = dataPager;
            this.appService=appService;
            input = new GetLanguageTextsInput()
            {
                FilterText = "",
                MaxResultCount = AppConsts.DefaultPageSize,
                SkipCount = 0, 
            };
            sources = new ObservableCollection<string>();
            baseLanguages = new ObservableCollection<LanguageStruct>();
            targetLanguages = new ObservableCollection<LanguageStruct>();
        }

        private LanguageStruct selectedBaseLanguage;
        private LanguageStruct selectedTargetLanguage;
        private ObservableCollection<string> sources;
        private ObservableCollection<LanguageStruct> baseLanguages;
        private ObservableCollection<LanguageStruct> targetLanguages;

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

        private async Task GetLanguageTexts(GetLanguageTextsInput filter)
        {
            await WebRequest.Execute(() => appService.GetLanguageTexts(filter), dataPager.SetList);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
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

            if (parameters.ContainsKey("Value"))
            {
                var applicationLanguage = parameters.GetValue<ApplicationLanguageListDto>("Value");

                var lang = context.Configuration.Localization.Languages.FirstOrDefault(t => t.Name.Equals(applicationLanguage.Name));
                if (lang != null)
                {
                    SelectedBaseLanguage = new LanguageStruct(lang.Icon, lang.Name, lang.DisplayName);
                    SelectedTargetLanguage = new LanguageStruct(lang.Icon, lang.Name, lang.DisplayName);
                }

                input.TargetValueFilter = "ALL";
                input.SourceName = Sources.Last();
            }
             
            await SetBusyAsync(async () => await GetLanguageTexts(input));
            base.OnNavigatedTo(parameters);
        }
    }
}