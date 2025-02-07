using System; 
using System.Text;

namespace AppFramework.Shared.Core
{
    public interface IMessenger
    {
        /// <summary>
        /// 根据订阅者的令牌发送消息
        /// </summary>
        /// <param name="subscriber">订阅者</param>
        /// <param name="token">令牌</param>
        void Send(string token);

        /// <summary>
        /// 根据订阅者的令牌发送带参数的消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subscriber">订阅者</param>
        /// <param name="token">令牌</param>
        /// <param name="message">消息内容</param>
        void Send<T>(string token, T message);

        /// <summary>
        /// 根据命令发送消息
        /// </summary>
        /// <param name="token">令牌</param>
        void SendByToken(string token);

        /// <summary>
        /// 根据命令发送带参数的消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token">令牌</param>
        /// <param name="message">消息内容</param>
        void SendByToken<T>(string token, T message);
         
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <param name="token">令牌</param>
        /// <param name="action">委托</param>
        void Subscribe(string token, Action action);

        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="token">令牌</param>
        /// <param name="action">带参数的委托</param>
        void Subscribe<T>(string token, Action<T> action);

        /// <summary>
        /// 取消订阅消息
        /// </summary>
        /// <param name="token">令牌</param>
        void Unsubscribe(string token);
    }
}
