using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace AppFramework.Admin.Behaviors
{
    public class PasswordBehavior : Behavior<PasswordBox>
    {
        protected override void OnAttached()
        {
            AssociatedObject.PasswordChanged += AssociatedObject_PasswordChanged;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.PasswordChanged -= AssociatedObject_PasswordChanged;
            base.OnDetaching();
        }

        private void AssociatedObject_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            string password = PasswordExtensions.GetPassword(passwordBox);

            if (passwordBox != null && password != passwordBox.Password)
                PasswordExtensions.SetPassword(passwordBox, passwordBox.Password);
        }
    }

    public class PasswordExtensions
    {
        public static string GetPassword(DependencyObject obj)
        {
            return (string)obj.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject obj, string value)
        {
            obj.SetValue(PasswordProperty, value);
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordExtensions), new PropertyMetadata(null, PropertyChangedCallback));

        public static void PropertyChangedCallback(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var passwordBox = sender as PasswordBox;
            string password = (string)args.NewValue;

            if (passwordBox != null && passwordBox.Password != password)
                passwordBox.Password = password;
        }
    }
}