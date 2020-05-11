using System;
using System.Collections.Generic;
using System.Text;

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
