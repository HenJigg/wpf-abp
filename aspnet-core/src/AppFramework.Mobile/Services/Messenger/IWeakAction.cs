using System;
using System.Collections.Generic;
using System.Text;

namespace AppFramework.Shared.Core
{
    public interface IWeakAction
    {
        /// <summary>
        /// 令牌
        /// </summary>
        string Token { get; }

        /// <summary>
        /// 事件类型
        /// </summary>
        string Type { get; }

        /// <summary>
        /// 是否可用
        /// </summary>
        bool IsAlive { get; }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="message">消息内容</param>
        void Execute(object message);

        /// <summary>
        /// 释放
        /// </summary>
        void Dispose();
    }
}
