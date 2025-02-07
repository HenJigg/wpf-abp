using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AppFramework.Admin.MaterialUI.Themes.Controls
{
    internal class TabCloseItem : TabItem
    {
        public ICommand? CloseCommand
        {
            get => (ICommand?)GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }
        public static readonly DependencyProperty CloseCommandProperty
            = DependencyProperty.Register(nameof(CloseCommand), typeof(ICommand), typeof(TabCloseItem), new PropertyMetadata(default(ICommand?)));

        public object? CloseCommandParameter
        {
            get => GetValue(CloseCommandParameterProperty);
            set => SetValue(CloseCommandParameterProperty, value);
        }
        public static readonly DependencyProperty CloseCommandParameterProperty
            = DependencyProperty.Register(nameof(CloseCommandParameter), typeof(object), typeof(TabCloseItem), new PropertyMetadata(default(object?)));

        public override void OnApplyTemplate()
        {
            if (GetTemplateChild("PART_DeleteButton") is Button closeButton)
            {
                closeButton.Click+=CloseButton_Click;
            }
            base.OnApplyTemplate();
        }

        private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            OnCloseClick();
            e.Handled = true;
        }
         
        public event RoutedEventHandler CloseClick
        {
            add => AddHandler(CloseClickEvent, value);
            remove => RemoveHandler(CloseClickEvent, value);
        }

        public static readonly RoutedEvent CloseClickEvent
            = EventManager.RegisterRoutedEvent(nameof(CloseClick), RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(TabCloseItem));
    
        protected virtual void OnCloseClick()
        {
            RaiseEvent(new RoutedEventArgs(CloseClickEvent, this));

            if (CloseCommand?.CanExecute(CloseCommandParameter) ?? false)
            {
                CloseCommand.Execute(CloseCommandParameter);
            }
        }
    }
}
