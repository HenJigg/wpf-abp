using Prism.Mvvm;
using System;

namespace AppFramework.Shared.Services.Permission
{
    public class PermissionItem : BindableBase
    {
        public PermissionItem(string key, string name, Action action)
        {
            Key = key;
            Name = name;
            Action = action;
        }

        public PermissionItem(string icon, string key, string name, Action action) : this(key, name, action)
        {
            Icon = icon;
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged(); }
        }

        public Action Action { get; }
        public string Key { get; set; }
        public string Icon { get; set; }
    }
}
