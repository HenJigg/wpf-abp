namespace AppFramework.ApiClient
{
    public static class DebugServerIpAddresses
    {
        /*
          * 此字段用于设置调试客户端的 IP 地址（例如：Xamarin 应用程序）
          * 它被设置在
          * - *.Mobile.Droid 项目中的 SplashActivity.cs（StartApplication 方法），
          * - *.Mobile.iOS 项目中的 AppDelegate.cs（FinishedLaunching 方法）。
          */
        public static string Current = "localhost";
    }
}