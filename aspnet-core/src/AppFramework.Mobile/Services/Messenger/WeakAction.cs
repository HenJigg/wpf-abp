using System;
using System.Collections.Generic;
using System.Text;

namespace AppFramework.Shared.Core
{
    public class WeakAction : IWeakAction
    {
        private Action _action;
        private string _token;
        private string _type;
        private bool _isAlive;

        public WeakAction(string token, Action action)
          : this(token, action, string.Empty, true)
        { }

        public WeakAction(string token, Action action, string typeName, bool isAlive)
        {
            _token = token;
            _type = typeName;
            _action = action;
            _isAlive = isAlive;
        }

        public string Token => _token;

        public bool IsAlive => _isAlive;

        public string Type => _type;

        public void Dispose()
        {
            _isAlive = false;
            _action = null;
        }

        public void Execute(object message)
        {
            _action?.Invoke();
        }
    }
}
