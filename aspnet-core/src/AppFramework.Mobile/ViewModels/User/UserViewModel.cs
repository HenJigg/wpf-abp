using AppFramework.Authorization.Users;
using AppFramework.Authorization.Users.Dto;
using System.Threading.Tasks; 

namespace AppFramework.Shared.ViewModels
{
    public class UserViewModel : NavigationMasterViewModel
    {
        #region 字段/属性

        private readonly IUserAppService appService;

        public GetUsersInput input { get; set; }

        public string FilterText
        {
            get { return input.Filter; }
            set
            {
                input.Filter = value;
                RaisePropertyChanged();
                AsyncRunner.Run(SearchWithDelayAsync(value));
            }
        }

        #endregion 字段/属性

        public UserViewModel(IUserAppService appService)
        {
            input = new GetUsersInput
            {
                Filter = "",
                MaxResultCount = AppConsts.DefaultPageSize,
                SkipCount = 0
            };
            this.appService=appService;
        }

        private async Task SearchWithDelayAsync(string filterText)
        {
            if (!string.IsNullOrEmpty(filterText))
            {
                await Task.Delay(1000);

                if (filterText != input.Filter)
                    return;
            }

            dataPager.SkipCount = 0;

            await RefreshAsync();
        }

        public override async Task RefreshAsync()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() => appService.GetUsers(input), dataPager.SetList);
            });
        } 
    }
}