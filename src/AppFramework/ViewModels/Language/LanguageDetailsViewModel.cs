using AppFramework.Localization;
using Prism.Services.Dialogs;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using AppFramework.Common;
using AppFramework.Localization.Dto;
using System.Collections.ObjectModel;
using System.Linq; 

namespace AppFramework.ViewModels
{
    public class LanguageDetailsViewModel : HostDialogViewModel
    {
        #region 字段/属性

        private readonly ILanguageAppService appService;
        private ApplicationLanguageEditDto language;

        private ComboboxItemDto selectedFlag;
        private ComboboxItemDto selectedLanguage;

        private ObservableCollection<ComboboxItemDto> flags;
        private ObservableCollection<ComboboxItemDto> languageNames;

        /// <summary>
        /// 可选语言列表
        /// </summary>
        public ObservableCollection<ComboboxItemDto> LanguageNames
        {
            get { return languageNames; }
            set { languageNames = value; RaisePropertyChanged(); }
        }

        /// <summary>
        /// 图标
        /// </summary>
        public ObservableCollection<ComboboxItemDto> Flags
        {
            get { return flags; }
            set { flags = value; RaisePropertyChanged(); }
        }

        //选中语言
        public ComboboxItemDto SelectedLanguage
        {
            get { return selectedLanguage; }
            set
            {
                selectedLanguage = value;
                if (selectedLanguage != null)
                {
                    language.Name = value.Value;
                }
                RaisePropertyChanged();
            }
        }

        //选中图标
        public ComboboxItemDto SelectedFlag
        {
            get { return selectedFlag; }
            set
            {
                selectedFlag = value;
                if (selectedLanguage != null)
                {
                    language.Icon = value.Value;
                }
                RaisePropertyChanged();
            }
        }

        //是否启用
        public bool IsEnabled
        {
            get { return language.IsEnabled; }
            set
            {
                language.IsEnabled = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public LanguageDetailsViewModel(ILanguageAppService appService)
        {
            language = new ApplicationLanguageEditDto();
            this.appService = appService;
        }

        protected override async void Save()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() =>
                appService.CreateOrUpdateLanguage(new CreateOrUpdateLanguageInput() { Language = language }),
                async () =>
                {
                    base.Save();
                    await Task.CompletedTask;
                });
            });
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await SetBusyAsync(async () =>
            {
                int? id = null;
                if (parameters.ContainsKey("Value"))
                    id = parameters.GetValue<ApplicationLanguageListDto>("Value").Id;

                await WebRequest.Execute(() =>
                        appService.GetLanguageForEdit(new NullableIdDto(id)),
                        GetLanguageForEditSuccessed);
            });
        }

        private async Task GetLanguageForEditSuccessed(GetLanguageForEditOutput output)
        {
            Flags = new ObservableCollection<ComboboxItemDto>(output.Flags);
            LanguageNames = new ObservableCollection<ComboboxItemDto>(output.LanguageNames);

            if (output.Language != null)
            {
                language.Id = output.Language.Id;
                language.Name = output.Language.Name;
                language.Icon = output.Language.Icon;
                IsEnabled = output.Language.IsEnabled;

                var f = Flags.FirstOrDefault(t => t.Value.Equals(language.Icon));
                if (f != null) SelectedFlag = f;

                var l = LanguageNames.FirstOrDefault(t => t.Value.Equals(language.Name));
                if (l != null) SelectedLanguage = l;
            }
            else
            {
                SelectedFlag = Flags.First();
                SelectedLanguage = LanguageNames.First();
            }

            await Task.CompletedTask;
        }
    }
}
