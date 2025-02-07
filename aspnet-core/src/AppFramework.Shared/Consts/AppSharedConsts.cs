namespace AppFramework.Shared
{
    public class AppSharedConsts
    {
        /// <summary>
        /// 默认的页面数量
        /// </summary>
        public static int DefaultPageSize = 20;

        /// <summary>
        /// 默认的查找等待时间
        /// </summary>
        public static int DefaultSearchDelayMilliseconds = 1000;

        /*
         * WPF客户端使用的常量定义
         */

        public const string AppName = "AppFramework";

        /// <summary>
        /// 登录页的唯一会话标识
        /// </summary>
        public const string LoginIdentifier = "Login";

        /// <summary>
        /// 首页的唯一会话标识 
        /// </summary>
        public const string RootIdentifier = "Root";

        /// <summary>
        /// 弹出窗口中的唯一会话标识
        /// </summary>
        public const string DialogIdentifier = "DialogRoot";

        /// <summary>
        /// 初始化页的唯一会话标识
        /// </summary>
        public const string SplashScreenIdentifier = "SplashScreen";
    }
}
