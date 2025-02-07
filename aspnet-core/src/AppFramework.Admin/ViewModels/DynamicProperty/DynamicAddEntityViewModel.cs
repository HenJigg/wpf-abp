using AppFramework.DynamicEntityProperties;
using AppFramework.Shared;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AppFramework.Admin.ViewModels
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
            set { model = value; OnPropertyChanged(); }
        }

        private string selectedItem;
        private readonly IDynamicEntityPropertyDefinitionAppService appService;

        public string SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; OnPropertyChanged(); }
        }

        public override async Task Save()
        {
            if (string.IsNullOrWhiteSpace(SelectedItem))
                return;

            base.Save(SelectedItem);

            await Task.CompletedTask;
        }

        private async Task GetAllEntities()
        {
            await appService.GetAllEntities()
                .WebAsync(async result =>
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
