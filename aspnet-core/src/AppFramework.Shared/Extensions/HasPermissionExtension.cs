using Prism.Ioc;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

namespace AppFramework.Shared
{
    public class HasPermissionExtension : MarkupExtension
    {
        /// <summary>
        /// 权限键
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 验证是否包含该权限
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns>是否符合权限: 显示/隐藏</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget target
               && target.TargetObject is FrameworkElement element
               && DesignerProperties.GetIsInDesignMode(element))
            {
                return Visibility.Visible;
            }

            if (string.IsNullOrWhiteSpace(Text))
                return Visibility.Collapsed;

            var permissionService = ContainerLocator.Container.Resolve<IPermissionService>();
            if (permissionService.HasPermission(Text))
                return Visibility.Visible;

            return Visibility.Collapsed;
        }
    }
}