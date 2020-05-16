/*
*
* 文件名    ：AuthItem                             
* 程序说明  : 权限值定义实体
* 更新时间  : 2020-05-16 15:01
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/

namespace Consumption.Core.Entity
{
    /// <summary>
    /// 权限
    /// </summary>
    public class AuthItem : BaseEntity
    {
        public string AuthName { get; set; }

        public string AuthValue { get; set; }
    }
}
