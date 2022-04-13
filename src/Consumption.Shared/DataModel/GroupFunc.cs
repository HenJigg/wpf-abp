namespace Consumption.Shared.DataModel
{
    using System.ComponentModel;

    /// <summary>
    /// 组功能
    /// </summary>
    public class GroupFunc : BaseEntity
    {
        /// <summary>
        /// 组代码
        /// </summary>
        public string GroupCode { get; set; }

        /// <summary>
        /// 菜单代码
        /// </summary>
        public string MenuCode { get; set; }

        /// <summary>
        /// 权限值
        /// </summary>
        public int Auth { get; set; }
    }
}
