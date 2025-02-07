using System;
using System.Collections.Generic;

namespace AppFramework.Shared.Core
{
    public class Messenger : IMessenger
    {
        private Dictionary<string, IWeakAction> _weakEvents;

        public void SendByToken(string token)
        {
            Send(string.Empty, token);
        }

        public void SendByToken<T>(string token, T message)
        {
            Send(token, message);
        }

        public void Send(string token)
        {
            _weakEvents[token].SendMessage();
        }

        public void Send<T>(string token, T message)
        {
            _weakEvents[token].SendMessage(message);
        }

        public void Subscribe(string token, Action action)
        {
            if (_weakEvents == null)
                _weakEvents = new Dictionary<string, IWeakAction>();

            _weakEvents[token] = new WeakAction(token, action);

            RefreshSubscribes(_weakEvents);
        }

        public void Subscribe<T>(string token, Action<T> action)
        {
            if (_weakEvents == null)
                _weakEvents = new Dictionary<string, IWeakAction>();

            _weakEvents[token] = new WeakAction<T>(token, action);

            RefreshSubscribes(_weakEvents);
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="subscriber">订阅者</param>
        /// <param name="token">令牌</param>
        public void Unsubscribe(string token)
        {
            if (string.IsNullOrWhiteSpace(token) ||
               _weakEvents == null ||
               _weakEvents.Count == 0 ||
               !_weakEvents.ContainsKey(token))
            {
                return;
            }

            if (_weakEvents.ContainsKey(token))
            {
                var weakMessages = _weakEvents[token];
                weakMessages?.Dispose();
            }

            RefreshSubscribes(_weakEvents);
        }

        private void RefreshSubscribes(IDictionary<string, IWeakAction> subscriberCollection)
        {
            if (subscriberCollection == null) return;

            var removeKeys = new List<string>();

            foreach (var subscriber in subscriberCollection)
            {
                if (!subscriber.Value.IsAlive)
                    removeKeys.Add(subscriber.Key);
            }

            foreach (var key in removeKeys)
            {
                subscriberCollection.Remove(key);
            }
        }
    }
}
