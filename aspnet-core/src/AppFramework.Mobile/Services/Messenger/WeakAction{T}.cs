using System;
using System.Collections.Generic;
using System.Text;

namespace AppFramework.Shared.Core
{
    public class WeakAction<T> : IWeakAction
    {
        private Action<T> _action;
        private string _token;
        private string _type;
        private bool _isAlive;

        public WeakAction(string token, Action<T> action)
            : this(token, action, typeof(T), true)
        { }

        public WeakAction(string token, Action<T> action, Type type, bool isAlive)
        {
            _token = token; 
            _action = action;
            _isAlive = isAlive;
            _type = type.FullName;
        }

        public string Token => _token;

        public string Type => _type;

        public bool IsAlive => _isAlive;

        public void Dispose()
        {
            _isAlive = false;
            _action = null;
        }

        public void Execute(object message)
        {
            _action?.Invoke((T)message);
        }
    }
}
