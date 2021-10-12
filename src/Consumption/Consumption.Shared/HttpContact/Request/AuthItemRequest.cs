using Consumption.Shared.HttpContact;

namespace Consumption.Core.Request
{
    /// <summary>
    /// 获取功能按钮请求
    /// </summary>
    public class AuthItemRequest : BaseRequest
    {
        public override string route { get => "api/AuthItem/GetAll"; }
    }
}
