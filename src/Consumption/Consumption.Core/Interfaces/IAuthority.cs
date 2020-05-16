/*
*
* 文件名    ：IAuthority                             
* 程序说明  : 程序功能权限接口
* 更新时间  : 2020-05-11
* 更新人    : zhouhaogg789@outlook.com
* 
*
*/
namespace Consumption.Core.Interfaces
{
    /// <summary>
    /// 权限接口
    /// </summary>
    public interface IAuthority
    {
        /// <summary>
        /// 获取按钮权限
        /// </summary>
        /// <param name="authValue"></param>
        /// <returns></returns>
        bool GetButtonAuth(int authValue);

        /// <summary>
        /// 加载模板权限
        /// </summary>
        void LoadModuleAuth();

        /// <summary>
        /// 权限值
        /// </summary>
        int? AuthValue { get; set; }
    }
}
