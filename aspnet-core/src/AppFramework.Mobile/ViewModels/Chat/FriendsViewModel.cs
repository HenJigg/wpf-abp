using AppFramework.ApiClient;
using AppFramework.Shared.Models.Chat;
using AppFramework.Shared.Services;
using AppFramework.Shared.Views;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.ListView.XForms;
using System.Threading.Tasks;

namespace AppFramework.Shared.ViewModels
{
    public class FriendsViewModel : RegionViewModel
    {
        public IFriendChatService chatService { get; private set; }

        public DelegateCommand<ItemTappedEventArgs> ClickChatCommand { get; private set; }

        public FriendsViewModel(IFriendChatService chatService)
        {
            this.chatService = chatService;

            ClickChatCommand = new DelegateCommand<ItemTappedEventArgs>(async arg =>
            {
                NavigationParameters param = new NavigationParameters();
                param.Add("Value", arg.ItemData);

                await navigationService.NavigateAsync(AppViews.FriendsChat, param);
            });
        }

        public override async Task RefreshAsync()
        {
            await chatService.GetUserChatFriendsAsync();
        }
    }
}
