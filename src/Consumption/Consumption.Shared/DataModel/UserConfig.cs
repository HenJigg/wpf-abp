namespace Consumption.Shared.DataModel
{
    /// <summary>
    /// 用户个性化配置
    /// </summary>
    public class UserConfig : BaseEntity
    {
        /// <summary>
        /// 账户名
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 预计支出
        /// </summary>
        public decimal ExpectedOut { get; set; }

        /// <summary>
        /// 预计收入
        /// </summary>
        public decimal ExpectedIn { get; set; }
    }
}
