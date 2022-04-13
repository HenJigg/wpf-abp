namespace Consumption.Shared.DataModel
{
    /// <summary>
    /// 权限
    /// </summary>
    public class AuthItem : BaseEntity
    {
        /// <summary>
        /// 权限定义名称
        /// </summary>
        public string AuthName { get; set; }

        /// <summary>
        /// 设定预期图标
        /// </summary>
        public string AuthKind { get; set; }

        /// <summary>
        /// 设定预期颜色
        /// </summary>
        public string AuthColor { get; set; }

        /// <summary>
        /// 所属权限值
        /// </summary>
        public int AuthValue { get; set; }
    }
}
