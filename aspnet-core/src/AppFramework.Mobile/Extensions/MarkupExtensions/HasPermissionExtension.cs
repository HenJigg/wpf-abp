using AppFramework.Shared;
using Prism.Ioc;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppFramework.Shared.Extensions.MarkupExtensions
{
    [ContentProperty("Text")]
    public class HasPermissionExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrWhiteSpace(Text))
            {
                return false;
            }

            var permissionService = ContainerLocator.Container.Resolve<IPermissionService>();
            return permissionService.HasPermission(Text);
        }
    }
}