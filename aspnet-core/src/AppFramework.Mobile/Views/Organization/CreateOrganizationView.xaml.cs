using Xamarin.Forms;
using Prism.Ioc;
using AppFramework.Shared.Core;

namespace AppFramework.Shared.Views
{
    public partial class CreateOrganizationView : ContentView
    {
        private readonly IMessenger messenger;

        public CreateOrganizationView()
        {
            InitializeComponent();
            messenger= ContainerLocator.Container.Resolve<IMessenger>(); 
            messenger.Sub<bool>("OrganizationViewIsVisible", OrganizationViewIsVisibleHandler);
        }

        public void OrganizationViewIsVisibleHandler(bool IsVisible)
        {
            if (IsVisible)
                this.Show();
            else
                this.Hide();
        }

        public void Show()
        {
            this.IsVisible = true;
            this.MainContent.FadeTo(1);
            this.MainContent.TranslateTo(this.MainContent.TranslationX, 0);
            this.ShadowView.IsVisible = true;
        }

        public void Hide()
        {
            this.ShadowView.IsVisible = false;
            var fadeAnimation = new Animation(v => this.MainContent.Opacity = v, 1, 0);
            var translateAnimation = new Animation(v => this.MainContent.TranslationY = v, 0, this.MainContent.Height, null, () => { this.IsVisible = false; });

            var parentAnimation = new Animation { { 0.5, 1, fadeAnimation }, { 0, 1, translateAnimation } };
            parentAnimation.Commit(this, "HideCreateOrganizationView");
        }

        private void CloseOrganizationView(object sender, System.EventArgs e)
        {
            this.Hide();
        }
    }
}