using System; 
using System.Threading.Tasks;

namespace AppFramework.Shared.Core
{
    public static class IMessengerExtensions
    {
        /// <summary>
        /// 发送消息通知(不带参数)
        /// </summary>
        /// <param name="messenger"></param>
        /// <param name="token">订阅者密钥</param>
        public static void Send(this IMessenger messenger,
            string token)
        { 
            messenger.Send(token);
        }

        /// <summary>
        /// 发送消息通知(带泛型参数)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messenger"></param>
        /// <param name="token">订阅者密钥</param>
        /// <param name="message"></param>
        public static void Send<T>(this IMessenger messenger,
           string token, T message)
        { 
            messenger.Send(token, message);
        }

        /// <summary>
        /// 订阅消息(无参数返回)
        /// </summary>
        /// <param name="messenger"></param>
        /// <param name="token">订阅者密钥</param>
        /// <param name="action"></param>
        public static void Sub(this IMessenger messenger,
            string token,
            Action action)
        { 
            messenger.Subscribe(token, action);
        }

        /// <summary>
        /// 订阅消息(异步无参数消息)
        /// </summary>
        /// <param name="messenger"></param>
        /// <param name="token">订阅者密钥</param>
        /// <param name="func"></param>
        public static void Sub(this IMessenger messenger,
            string token,
            Func<Task> func)
        { 
            messenger.Subscribe(token, async () => await func());
        }

        /// <summary>
        /// 订阅消息(无参数返回)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messenger"></param>
        /// <param name="token">订阅者密钥</param>
        /// <param name="action"></param>
        public static void Sub<T>(this IMessenger messenger,
            string token,
            Action<T> action)
        { 
            messenger.Subscribe(token, action);
        }

        /// <summary>
        /// 订阅消息(异步带参数)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="messenger"></param>
        /// <param name="token">订阅者密钥</param>
        /// <param name="func"></param>
        public static void Sub<T>(this IMessenger messenger,
            string token,
            Func<Task<T>> func)
        { 
            messenger.Subscribe(token, async () => await func());
        }

        /// <summary>
        /// 取消订阅消息
        /// </summary>
        /// <param name="messenger"></param>
        /// <param name="token">订阅者密钥</param>
        public static void UnSub(this IMessenger messenger, string token)
        {
            messenger.Unsubscribe(token);
        } 
    }
}
