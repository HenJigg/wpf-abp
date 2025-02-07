using System;

namespace AppFramework.Shared
{
    public class SharedConsts
    {
        /// <summary>
        /// 默认的页面数量
        /// </summary>
        public static int DefaultPageSize = 20;

        /// <summary>
        /// 默认的查找等待时间
        /// </summary>
        public static int DefaultSearchDelayMilliseconds = 1000;

        /// <summary>
        /// 默认的文件路径
        /// </summary>
        public static string DocumentPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    }
}
