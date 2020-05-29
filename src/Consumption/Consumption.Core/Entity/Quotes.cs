/*
*
* 文件名    ：Quotes                             
* 程序说明  : 名言警句
* 更新时间  : 2020-05-16 15:53
* 更新人    : zhouhaogg789@outlook.com
* 
*
* 作用      : 主要用于推送每日一个励志感人的段子, 让你找到花钱的理由。 
*            
*
*/

namespace Consumption.Core.Entity
{
    /// <summary>
    /// 名言警句
    /// </summary>
    public class Quotes : BaseEntity
    {
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}
