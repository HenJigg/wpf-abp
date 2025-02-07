using System.Threading.Tasks;

namespace AppFramework.Shared.Services
{
    /// <summary>
    /// 应用程序服务
    /// </summary>
    public interface IApplicationService
    {
        /// <summary>
        /// 头像
        /// </summary>
        byte[] Photo { get; set; }

        /// <summary>
        /// 用户名和姓氏
        /// </summary>
        string UserNameAndSurname { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        string EmailAddress { get; set; }

        /// <summary>
        /// 应用程序信息
        /// </summary>
        string ApplicationInfo { get; set; }

        /// <summary>
        /// 应用程序名称
        /// </summary>
        string ApplicationName { get; set; }

        /// <summary>
        /// 更改个人资料照片
        /// </summary>
        void ChangeProfilePhoto();

        /// <summary>
        /// 显示个人资料照片
        /// </summary>
        /// <returns></returns>
        Task ShowProfilePhoto();

        /// <summary>
        /// 显示个人信息
        /// </summary>
        /// <returns></returns>
        Task ShowMyProfile();

        /// <summary>
        /// 设置应用程序信息
        /// 备注: 用户名、头像、权限、应用程序名称版本等
        /// </summary>
        Task GetApplicationInfo();

        /// <summary>
        /// 刷新菜单
        /// </summary>
        void RefreshAuthMenus();

        /// <summary>
        /// 执行用户功能请求
        /// </summary>
        /// <param name="key"></param>
        void ExecuteUserAction(string key);
    }
}