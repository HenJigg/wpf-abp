using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime; 
using AppFramework.Shared;
using AppFramework.Shared.Core.Behaviors;
using Flurl.Http;
using Xamarin.Forms.Platform.Android; 

namespace AppFramework.Droid
{
    [Activity(Label = "AppFramework",
        Icon = "@mipmap/icon",
        Theme = "@style/MainTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : FormsAppCompatActivity
    {
        public event OnBackPressedHandler OnBackPressedEventHandler;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            UserDialogs.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            App application = new App();
            OnBackPressedEventHandler+=application.OnBackPressedHandler;
            LoadApplication(application);
        }
          
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override void OnBackPressed()
        {
            var result = OnBackPressedEventHandler?.Invoke();
            if ((bool)result) base.OnBackPressed();
        } 
    }
}