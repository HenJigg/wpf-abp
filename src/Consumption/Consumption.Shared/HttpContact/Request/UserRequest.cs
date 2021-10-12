namespace Consumption.Core.Request
{
    using Consumption.Shared.Dto;
    using Consumption.Shared.HttpContact;

    /// <summary>
    /// 用户登录请求
    /// </summary>
    public class UserLoginRequest : BaseRequest
    {
        public override string route { get => "api/User/Login"; }

        public LoginDto Parameter { get; set; }
    }

    /// <summary>
    /// 用户权限请求
    /// </summary>
    public class UserPermRequest : BaseRequest
    {
        public override string route { get => "api/User/Perm"; }

        public string account { get; set; }
    }

}
