using AppFramework.Shared;
using AppFramework.Admin.Models;
using AppFramework.Localization;
using AppFramework.Localization.Dto;
using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace AppFramework.Admin.ViewModels
{
    public class LanguageTextDetailsViewModel : HostDialogViewModel
    {
        #region 字段/属性

        private string key, baseValue, targetValue;

        public string Key
        {
            get { return key; }
            set { key = value; OnPropertyChanged(); }
        }

        public string BaseValue
        {
            get { return baseValue; }
            set { baseValue = value; OnPropertyChanged(); }
        }

        public string TargetValue
        {
            get { return targetValue; }
            set { targetValue = value; OnPropertyChanged(); }
        }

        private LanguageStruct baseLanguage;
        private LanguageStruct targetLanguage;
        private readonly ILanguageAppService appService;

        public LanguageStruct BaseLanguage
        {
            get { return baseLanguage; }
            set { baseLanguage = value; OnPropertyChanged(); }
        }

        public LanguageStruct TargetLanguage
        {
            get { return targetLanguage; }
            set { targetLanguage = value; OnPropertyChanged(); }
        }

        private string SourceName;

        #endregion

        public LanguageTextDetailsViewModel(ILanguageAppService appService)
        {
            this.appService = appService;
        }

        public override async Task Save()
        {
            await SetBusyAsync(async () =>
            {
                await appService.UpdateLanguageText(new UpdateLanguageTextInput()
                {
                    Key = Key,
                    LanguageName = BaseLanguage.Name,
                    SourceName = SourceName,
                    Value = TargetValue
                })
                .WebAsync(base.Save);
            });
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                SourceName = parameters.GetValue<string>("SourceName");
                BaseLanguage = parameters.GetValue<LanguageStruct>("Base");
                TargetLanguage = parameters.GetValue<LanguageStruct>("Target");

                var lang = parameters.GetValue<LanguageTextListDto>("Value");

                Key = lang.Key;
                BaseValue = lang.BaseValue;
                TargetValue = lang.TargetValue;
            }
        }
    }
}
