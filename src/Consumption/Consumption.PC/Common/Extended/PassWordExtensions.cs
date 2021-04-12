using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;

namespace Consumption.PC.Common.Extended
{
    public class PassWordExtensions
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password",
            typeof(string), typeof(PassWordExtensions),
            new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            string password = (string)e.NewValue;
            if (passwordBox != null && passwordBox.Password != password)
                passwordBox.Password = password;
        }

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }
    }

    public class PasswordBoxBehavior : Behavior<PasswordBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PasswordChanged += OnPasswordChanged;
        }

        private static void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            string password = PassWordExtensions.GetPassword(passwordBox);
            if (passwordBox != null && passwordBox.Password != password)
                PassWordExtensions.SetPassword(passwordBox, passwordBox.Password);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PasswordChanged -= OnPasswordChanged;
        }
    }
}
