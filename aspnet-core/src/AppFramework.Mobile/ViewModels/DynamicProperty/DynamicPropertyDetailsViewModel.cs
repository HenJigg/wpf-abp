using AppFramework.DynamicEntityProperties;
using AppFramework.DynamicEntityProperties.Dto;
using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;
using AppFramework.Shared.Core;
using AppFramework.Shared.Models;
using AppFramework.Shared.Services.Messenger;

namespace AppFramework.Shared.ViewModels
{
    public class DynamicPropertyDetailsViewModel : NavigationDetailViewModel
    {
        private readonly IMessenger messenger;
        private readonly IDynamicPropertyAppService appService;
          
        private DynamicPropertyModel model;

        public DynamicPropertyModel Model
        {
            get { return model; }
            set { model = value; RaisePropertyChanged(); }
        }

        public DynamicPropertyDetailsViewModel(IMessenger messenger,
            IDynamicPropertyAppService appService)
        {
            this.messenger = messenger;
            this.appService = appService;

            Model = new DynamicPropertyModel(); 
        }

        public override async void Save()
        {
            await SetBusyAsync(async () =>
             {
                 var input = Map<DynamicPropertyDto>(Model);

                 await WebRequest.Execute(async () =>
                 {
                     if (input.Id > 0)
                         await appService.Update(input);
                     else
                         await appService.Add(input);

                 }, async () => await GoBackAsync());
             });
        }

        public override async Task GoBackAsync()
        {
            messenger.Send(AppMessengerKeys.Dynamic);
            await base.GoBackAsync();
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                var property = parameters.GetValue<DynamicPropertyDto>("Value");
                Model = Map<DynamicPropertyModel>(property);
            }
        }
    }
}