using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Consumption.Common
{
    public class DialogServices : IDialogService
    {
        private readonly IContainerExtension _containerExtension;

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogService"/> class.
        /// </summary>
        /// <param name="containerExtension"></param>
        public DialogServices(IContainerExtension containerExtension)
        {
            _containerExtension = containerExtension;
        }

        /// <summary>
        /// Shows a non-modal dialog.
        /// </summary>
        /// <param name="name">The name of the dialog to show.</param>
        /// <param name="parameters">The parameters to pass to the dialog.</param>
        /// <param name="callback">The action to perform when the dialog is closed.</param>
        public void Show(string name, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            ShowDialogInternal(name, parameters, callback, false);
        }

        /// <summary>
        /// Shows a non-modal dialog.
        /// </summary>
        /// <param name="name">The name of the dialog to show.</param>
        /// <param name="parameters">The parameters to pass to the dialog.</param>
        /// <param name="callback">The action to perform when the dialog is closed.</param>
        /// <param name="windowName">The name of the hosting window registered with the IContainerRegistry.</param>
        public void Show(string name, IDialogParameters parameters, Action<IDialogResult> callback, string windowName)
        {
            ShowDialogInternal(name, parameters, callback, false, windowName);
        }

        /// <summary>
        /// Shows a modal dialog.
        /// </summary>
        /// <param name="name">The name of the dialog to show.</param>
        /// <param name="parameters">The parameters to pass to the dialog.</param>
        /// <param name="callback">The action to perform when the dialog is closed.</param>
        public void ShowDialog(string name, IDialogParameters parameters, Action<IDialogResult> callback)
        {
            ShowDialogInternal(name, parameters, callback, true);
        }

        /// <summary>
        /// Shows a modal dialog.
        /// </summary>
        /// <param name="name">The name of the dialog to show.</param>
        /// <param name="parameters">The parameters to pass to the dialog.</param>
        /// <param name="callback">The action to perform when the dialog is closed.</param>
        /// <param name="windowName">The name of the hosting window registered with the IContainerRegistry.</param>
        public void ShowDialog(string name, IDialogParameters parameters, Action<IDialogResult> callback, string windowName)
        {
            ShowDialogInternal(name, parameters, callback, true, windowName);
        }

        void ShowDialogInternal(string name, IDialogParameters parameters, Action<IDialogResult> callback, bool isModal, string windowName = null)
        {
            if (parameters == null)
                parameters = new DialogParameters();

            IDialogWindows dialogWindow = CreateDialogWindow(windowName);
            ConfigureDialogWindowEvents(dialogWindow, callback);
            ConfigureDialogWindowContent(name, dialogWindow, parameters);

            ShowDialogWindow(dialogWindow, isModal);
        }

        /// <summary>
        /// Shows the dialog window.
        /// </summary>
        /// <param name="dialogWindow">The dialog window to show.</param>
        /// <param name="isModal">If true; dialog is shown as a modal</param>
        protected virtual void ShowDialogWindow(IDialogWindows dialogWindow, bool isModal)
        {
            if (isModal)
                dialogWindow.ShowDialog();
            else
                dialogWindow.Show();
        }

        /// <summary>
        /// Create a new <see cref="IDialogWindowx"/>.
        /// </summary>
        /// <param name="name">The name of the hosting window registered with the IContainerRegistry.</param>
        /// <returns>The created <see cref="IDialogWindowx"/>.</returns>
        protected virtual IDialogWindows CreateDialogWindow(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return _containerExtension.Resolve<IDialogWindows>();
            else
                return _containerExtension.Resolve<IDialogWindows>(name);
        }

        /// <summary>
        /// Configure <see cref="IDialogWindowx"/> content.
        /// </summary>
        /// <param name="dialogName">The name of the dialog to show.</param>
        /// <param name="window">The hosting window.</param>
        /// <param name="parameters">The parameters to pass to the dialog.</param>
        protected virtual void ConfigureDialogWindowContent(string dialogName, IDialogWindows window, IDialogParameters parameters)
        {
            var content = _containerExtension.Resolve<object>(dialogName);
            if (!(content is FrameworkElement dialogContent))
                throw new NullReferenceException("A dialog's content must be a FrameworkElement");

            MvvmHelpers.AutowireViewModel(dialogContent);

            if (!(dialogContent.DataContext is IDialogAware viewModel))
                throw new NullReferenceException("A dialog's ViewModel must implement the IDialogAware interface");

            ConfigureDialogWindowProperties(window, dialogContent, viewModel);

            MvvmHelpers.ViewAndViewModelAction<IDialogAware>(viewModel, d => d.OnDialogOpened(parameters));
        }

        /// <summary>
        /// Configure <see cref="IDialogWindowx"/> and <see cref="IDialogAware"/> events.
        /// </summary>
        /// <param name="dialogWindow">The hosting window.</param>
        /// <param name="callback">The action to perform when the dialog is closed.</param>
        protected virtual void ConfigureDialogWindowEvents(IDialogWindows dialogWindow, Action<IDialogResult> callback)
        {
            Action<IDialogResult> requestCloseHandler = null;
            requestCloseHandler = (o) =>
            {
                dialogWindow.Result = o;
                dialogWindow.Close();
            };

            RoutedEventHandler loadedHandler = null;
            loadedHandler = (o, e) =>
            {
                dialogWindow.Loaded -= loadedHandler;
                dialogWindow.GetDialogViewModel().RequestClose += requestCloseHandler;
            };
            dialogWindow.Loaded += loadedHandler;

            CancelEventHandler closingHandler = null;
            closingHandler = (o, e) =>
            {
                if (!dialogWindow.GetDialogViewModel().CanCloseDialog())
                    e.Cancel = true;
            };
            dialogWindow.Closing += closingHandler;

            EventHandler closedHandler = null;
            closedHandler = (o, e) =>
            {
                dialogWindow.Closed -= closedHandler;
                dialogWindow.Closing -= closingHandler;
                dialogWindow.GetDialogViewModel().RequestClose -= requestCloseHandler;

                dialogWindow.GetDialogViewModel().OnDialogClosed();

                if (dialogWindow.Result == null)
                    dialogWindow.Result = new DialogResult();

                callback?.Invoke(dialogWindow.Result);

                dialogWindow.DataContext = null;
                dialogWindow.Content = null;
            };
            dialogWindow.Closed += closedHandler;
        }

        /// <summary>
        /// Configure <see cref="IDialogWindowx"/> properties.
        /// </summary>
        /// <param name="window">The hosting window.</param>
        /// <param name="dialogContent">The dialog to show.</param>
        /// <param name="viewModel">The dialog's ViewModel.</param>
        protected virtual void ConfigureDialogWindowProperties(IDialogWindows window, FrameworkElement dialogContent, IDialogAware viewModel)
        {
            var windowStyle = Dialog.GetWindowStyle(dialogContent);

            if (windowStyle == null)
                windowStyle = Prism.Services.Dialogs.Dialog.GetWindowStyle(dialogContent);

            if (windowStyle != null)
                window.Style = windowStyle;

            window.Content = dialogContent;
            window.DataContext = viewModel; //we want the host window and the dialog to share the same data context

            if (window.Owner == null)
                window.Owner = Application.Current?.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
        }
    }

    internal static class IDialogWindowExtensions
    {
        /// <summary>
        /// Get the <see cref="IDialogAware"/> ViewModel from a <see cref="IDialogWindowx"/>.
        /// </summary>
        /// <param name="dialogWindow"><see cref="IDialogWindowx"/> to get ViewModel from.</param>
        /// <returns>ViewModel as a <see cref="IDialogAware"/>.</returns>
        internal static IDialogAware GetDialogViewModel(this IDialogWindows dialogWindow)
        {
            return (IDialogAware)dialogWindow.DataContext;
        }
    }

    public static class MvvmHelpers
    {
        internal static void AutowireViewModel(object viewOrViewModel)
        {
            if (viewOrViewModel is FrameworkElement view && view.DataContext is null && ViewModelLocator.GetAutoWireViewModel(view) is null)
            {
                ViewModelLocator.SetAutoWireViewModel(view, true);
            }
        }

        public static void ViewAndViewModelAction<T>(object view, Action<T> action) where T : class
        {
            if (view is T viewAsT)
                action(viewAsT);

            if (view is FrameworkElement element && element.DataContext is T viewModelAsT)
            {
                action(viewModelAsT);
            }
        }
    }

}
