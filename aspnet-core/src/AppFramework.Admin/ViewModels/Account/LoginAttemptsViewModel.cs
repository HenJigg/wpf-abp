using AppFramework.Authorization.Users;
using AppFramework.Authorization.Users.Dto;
using AppFramework.Shared;
using AppFramework.Shared.Services;
using AppFramework.Admin.ViewModels.Shared;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Threading.Tasks;

namespace AppFramework.Admin.ViewModels
{
    public class LoginAttemptsViewModel : NavigationCurdViewModel
    {
        private readonly IUserLoginAppService appService;
        private GetLoginAttemptsInput input;
        public DelegateCommand SearchCommand { get; private set; }

        public DateTime? StartDate
        {
            get { return input.StartDate; }
            set { input.StartDate = value; OnPropertyChanged(); }
        }

        public DateTime? EndDate
        {
            get { return input.EndDate; }
            set { input.EndDate = value; OnPropertyChanged(); }
        }

        public string FilterText
        {
            get { return input.Filter; }
            set
            {
                input.Filter = value;
                OnPropertyChanged();
                Search();
            }
        }
         
        public LoginAttemptsViewModel(IUserLoginAppService appService)
        {
            this.appService = appService;
            Title = Local.Localize("LoginAttempts");

            input = new GetLoginAttemptsInput()
            {
                Filter = "",
                MaxResultCount = AppConsts.DefaultPageSize,
                SkipCount = 0
            };

            StartDate=DateTime.Now.AddDays(-3);
            EndDate=DateTime.Now;

            SearchCommand = new DelegateCommand(Search);
            dataPager.OnPageIndexChangedEventhandler += UsersOnPageIndexChangedEventhandler;
        }

        private void Search()
        {
            dataPager.PageIndex = 0;
        }

        private async void UsersOnPageIndexChangedEventhandler(object sender, PageIndexChangedEventArgs e)
        {
            input.StartDate = StartDate.GetFirstDate();
            input.EndDate = EndDate.GetLastDate(); 
            input.SkipCount = e.SkipCount;
            input.MaxResultCount = e.PageSize;

            await GetUserLogins(input);
        }

        private async Task GetUserLogins(GetLoginAttemptsInput filter)
        {
            await SetBusyAsync(async () =>
            {
                await appService.GetUserLoginAttempts(filter).WebAsync(dataPager.SetList);
            }); 
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            await GetUserLogins(input);
        }
    }
}
