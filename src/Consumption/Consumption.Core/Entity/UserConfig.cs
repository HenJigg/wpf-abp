/*
*
* 文件名    ：UserConfig                             
* 程序说明  : 用户个性化配置
* 更新时间  : 2020-05-16 15:05
* 更新人    : zhouhaogg789@outlook.com
*
* 作用      : 用于配置单个用户得每月预计支出 和 收入 
*/

namespace Consumption.Core.Entity
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
