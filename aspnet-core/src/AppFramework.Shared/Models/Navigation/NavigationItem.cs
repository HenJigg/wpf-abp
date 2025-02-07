using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace AppFramework.Shared.Models
{
    public class NavigationItem : BindableBase
    {
        public NavigationItem()
        { }

        public NavigationItem(
            string icon,
            string title,
            string pageViewName,
            string requiredPermissionName,
            ObservableCollection<NavigationItem> items = null)
        {
            Icon = icon;
            Title = title;
            PageViewName = pageViewName;
            RequiredPermissionName = requiredPermissionName;
            Items = items;
        }

        private bool isSelected;
        private ObservableCollection<NavigationItem> items;

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 页面名称
        /// </summary>
        public string PageViewName { get; set; }

        /// <summary>
        /// 导航参数
        /// </summary>
        public object NavigationParameter { get; set; }

        /// <summary>
        /// 权限名
        /// </summary>
        public string RequiredPermissionName { get; set; }

        /// <summary>
        /// 导航菜单列表
        /// </summary>
        public ObservableCollection<NavigationItem> Items
        {
            get { return items; }
            set { items = value; RaisePropertyChanged(); }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; RaisePropertyChanged(); }
        }
    }
}