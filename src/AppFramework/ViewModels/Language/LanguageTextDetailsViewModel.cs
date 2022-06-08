using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Localization;
using AppFramework.Localization.Dto;
using Prism.Services.Dialogs;

namespace AppFramework.ViewModels
{
    public class LanguageTextDetailsViewModel : HostDialogViewModel
    {
        #region 字段/属性

        private string key, baseValue, targetValue;

        public string Key
        {
            get { return key; }
            set { key = value; RaisePropertyChanged(); }
        }

        public string BaseValue
        {
            get { return baseValue; }
            set { baseValue = value; RaisePropertyChanged(); }
        }

        public string TargetValue
        {
            get { return targetValue; }
            set { targetValue = value; RaisePropertyChanged(); }
        }

        private LanguageStruct baseLanguage;
        private LanguageStruct targetLanguage;
        private readonly ILanguageAppService appService;

        public LanguageStruct BaseLanguage
        {
            get { return baseLanguage; }
            set { baseLanguage = value; RaisePropertyChanged(); }
        }

        public LanguageStruct TargetLanguage
        {
            get { return targetLanguage; }
            set { targetLanguage = value; RaisePropertyChanged(); }
        }

        private string SourceName;

        #endregion

        public LanguageTextDetailsViewModel(ILanguageAppService appService)
        {
            this.appService = appService;
        }

        protected override async void Save()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.UpdateLanguageText(new UpdateLanguageTextInput()
                {
                    Key = Key,
                    LanguageName = BaseLanguage.Name,
                    SourceName = SourceName,
                    Value = TargetValue
                }), async () =>
                {
                    base.Save();
                    await System.Threading.Tasks.Task.CompletedTask;
                });
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
