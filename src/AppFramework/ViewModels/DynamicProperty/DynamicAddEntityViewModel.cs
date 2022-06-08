using AppFramework.Common;
using AppFramework.DynamicEntityProperties;
using Prism.Services.Dialogs; 
using System.Collections.ObjectModel; 
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class DynamicAddEntityViewModel : HostDialogViewModel
    {
        public DynamicAddEntityViewModel(IDynamicEntityPropertyDefinitionAppService appService)
        {
            model = new ObservableCollection<string>();
            this.appService = appService;
        }

        private ObservableCollection<string> model;

        public ObservableCollection<string> Models
        {
            get { return model; }
            set { model = value; RaisePropertyChanged(); }
        }

        private string selectedItem;
        private readonly IDynamicEntityPropertyDefinitionAppService appService;

        public string SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; RaisePropertyChanged(); }
        }

        protected override void Save()
        {
            if (string.IsNullOrWhiteSpace(SelectedItem))
                return;

            base.Save(SelectedItem);
        }

        private async Task GetAllEntities()
        {
            await WebRequest.Execute(() => appService.GetAllEntities(),
                async result =>
                {
                    foreach (var item in result)
                        Models.Add(item);

                    await Task.CompletedTask;
                });
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await SetBusyAsync(GetAllEntities);
        }
    }
}
