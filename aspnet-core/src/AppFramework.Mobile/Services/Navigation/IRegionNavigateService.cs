using System;

namespace AppFramework.Shared.Services.Navigation
{
    /// <summary>
    /// 区域导航服务接口
    /// 包含: 指定区域导航、导航日志、导航回调
    /// </summary>
    public interface IRegionNavigateService
    {
        /// <summary>
        /// 指定区域进行页面导航
        /// </summary>
        /// <param name="regionName">区域名称</param>
        /// <param name="pageName">页面名称</param>
        void Navigate(string regionName, string pageName);

        /// <summary>
        /// 清空
        /// </summary>
        void Clear();

        /// <summary>
        /// 向后导航
        /// </summary>
        void GoBack();

        /// <summary>
        /// 向前导航
        /// </summary>
        void GoForward();
    }
}