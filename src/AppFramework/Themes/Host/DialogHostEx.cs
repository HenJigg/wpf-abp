using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace AppFramework.WindowHost
{
    /// <summary>
    /// Helper extensions for showing dialogs.
    /// </summary>
    public static class DialogHostEx
    {
        /// <summary>
        /// Shows a dialog using the first found <see cref="DialogHost"/> in a given <see cref="Window"/>.
        /// </summary>
        /// <param name="window">Window on which the modal dialog should be displayed. Must contain a <see cref="DialogHost"/>.</param>
        /// <param name="content"></param>
        /// <exception cref="InvalidOperationException">
        /// Thrown is a <see cref="DialogHost"/> is not found when conducting a depth first traversal of visual tree.
        /// </exception>
        /// <remarks>
        /// As a depth first traversal of the window's visual tree is performed, it is not safe to use this method in a situtation where a screen has multiple <see cref="DialogHost"/>s.
        /// </remarks>
        /// <returns></returns>
        public static async Task<object?> ShowDialog(this Window window, object content)
            => await GetFirstDialogHost(window).ShowInternal(content, null, null);

        /// <summary>
        /// Shows a dialog using the first found <see cref="DialogHost"/> in a given <see cref="Window"/>.
        /// </summary>
        /// <param name="window">Window on which the modal dialog should be displayed. Must contain a <see cref="DialogHost"/>.</param>
        /// <param name="content">Content to show (can be a control or view model).</param>
        /// <param name="openedEventHandler">Allows access to opened event which would otherwise have been subscribed to on a instance.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown is a <see cref="DialogHost"/> is not found when conducting a depth first traversal of visual tree.
        /// </exception>
        /// <remarks>
        /// As a depth first traversal of the window's visual tree is performed, it is not safe to use this method in a situtation where a screen has multiple <see cref="DialogHost"/>s.
        /// </remarks>
        /// <returns></returns>
        public static async Task<object?> ShowDialog(this Window window, object content, DialogOpenedEventHandler openedEventHandler)
            => await GetFirstDialogHost(window).ShowInternal(content, openedEventHandler, null);

        /// <summary>
        /// Shows a dialog using the first found <see cref="DialogHost"/> in a given <see cref="Window"/>.
        /// </summary>
        /// <param name="window">Window on which the modal dialog should be displayed. Must contain a <see cref="DialogHost"/>.</param>
        /// <param name="content">Content to show (can be a control or view model).</param>
        /// <param name="closingEventHandler">Allows access to closing event which would otherwise have been subscribed to on a instance.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown is a <see cref="DialogHost"/> is not found when conducting a depth first traversal of visual tree.
        /// </exception>
        /// <remarks>
        /// As a depth first traversal of the window's visual tree is performed, it is not safe to use this method in a situtation where a screen has multiple <see cref="DialogHost"/>s.
        /// </remarks>
        /// <returns></returns>
        public static async Task<object?> ShowDialog(this Window window, object content, DialogClosingEventHandler closingEventHandler)
            => await GetFirstDialogHost(window).ShowInternal(content, null, closingEventHandler);

        /// <summary>
        /// Shows a dialog using the first found <see cref="DialogHost"/> in a given <see cref="Window"/>.
        /// </summary>
        /// <param name="window">Window on which the modal dialog should be displayed. Must contain a <see cref="DialogHost"/>.</param>
        /// <param name="content">Content to show (can be a control or view model).</param>
        /// <param name="openedEventHandler">Allows access to opened event which would otherwise have been subscribed to on a instance.</param>
        /// <param name="closingEventHandler">Allows access to closing event which would otherwise have been subscribed to on a instance.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown is a <see cref="DialogHost"/> is not found when conducting a depth first traversal of visual tree.
        /// </exception>
        /// <remarks>
        /// As a depth first traversal of the window's visual tree is performed, it is not safe to use this method in a situtation where a screen has multiple <see cref="DialogHost"/>s.
        /// </remarks>
        /// <returns></returns>
        public static async Task<object?> ShowDialog(this Window window, object content, DialogOpenedEventHandler openedEventHandler, DialogClosingEventHandler closingEventHandler)
            => await GetFirstDialogHost(window).ShowInternal(content, openedEventHandler, closingEventHandler);

        /// <summary>
        /// Shows a dialog using the parent/ancestor <see cref="DialogHost"/> of the a given <see cref="DependencyObject"/>.
        /// </summary>
        /// <param name="childDependencyObject">Dependency object which should be a visual child of a <see cref="DialogHost"/>, where the dialog will be shown.</param>
        /// <param name="content">Content to show (can be a control or view model).</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown is a <see cref="DialogHost"/> is not found when conducting a depth first traversal of visual tree.
        /// </exception>
        /// <returns></returns>
        public static async Task<object?> ShowDialog(this DependencyObject childDependencyObject, object content)
            => await GetOwningDialogHost(childDependencyObject).ShowInternal(content, null, null);

        /// <summary>
        /// Shows a dialog using the parent/ancestor <see cref="DialogHost"/> of the a given <see cref="DependencyObject"/>.
        /// </summary>
        /// <param name="childDependencyObject">Dependency object which should be a visual child of a <see cref="DialogHost"/>, where the dialog will be shown.</param>
        /// <param name="content">Content to show (can be a control or view model).</param>
        /// <param name="openedEventHandler">Allows access to opened event which would otherwise have been subscribed to on a instance.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown is a <see cref="DialogHost"/> is not found when conducting a depth first traversal of visual tree.
        /// </exception>
        /// <returns></returns>
        public static async Task<object?> ShowDialog(this DependencyObject childDependencyObject, object content, DialogOpenedEventHandler openedEventHandler)
            => await GetOwningDialogHost(childDependencyObject).ShowInternal(content, openedEventHandler, null);

        /// <summary>
        /// Shows a dialog using the parent/ancestor <see cref="DialogHost"/> of the a given <see cref="DependencyObject"/>.
        /// </summary>
        /// <param name="childDependencyObject">Dependency object which should be a visual child of a <see cref="DialogHost"/>, where the dialog will be shown.</param>
        /// <param name="content">Content to show (can be a control or view model).</param>
        /// <param name="closingEventHandler">Allows access to closing event which would otherwise have been subscribed to on a instance.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown is a <see cref="DialogHost"/> is not found when conducting a depth first traversal of visual tree.
        /// </exception>
        /// <returns></returns>
        public static async Task<object?> ShowDialog(this DependencyObject childDependencyObject, object content, DialogClosingEventHandler closingEventHandler)
            => await GetOwningDialogHost(childDependencyObject).ShowInternal(content, null, closingEventHandler);

        /// <summary>
        /// Shows a dialog using the parent/ancestor <see cref="DialogHost"/> of the a given <see cref="DependencyObject"/>.
        /// </summary>
        /// <param name="childDependencyObject">Dependency object which should be a visual child of a <see cref="DialogHost"/>, where the dialog will be shown.</param>
        /// <param name="content">Content to show (can be a control or view model).</param>
        /// <param name="openedEventHandler">Allows access to opened event which would otherwise have been subscribed to on a instance.</param>
        /// <param name="closingEventHandler">Allows access to closing event which would otherwise have been subscribed to on a instance.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown is a <see cref="DialogHost"/> is not found when conducting a depth first traversal of visual tree.
        /// </exception>
        /// <returns></returns>
        public static async Task<object?> ShowDialog(this DependencyObject childDependencyObject, object content, DialogOpenedEventHandler openedEventHandler, DialogClosingEventHandler closingEventHandler)
            => await GetOwningDialogHost(childDependencyObject).ShowInternal(content, openedEventHandler, closingEventHandler);

        private static DialogHost GetFirstDialogHost(Window window)
        {
            if (window is null) throw new ArgumentNullException(nameof(window));

            DialogHost? dialogHost = VisualDepthFirstTraversal(window).OfType<DialogHost>().FirstOrDefault();

            if (dialogHost is null)
                throw new InvalidOperationException("Unable to find a DialogHost in visual tree");

            static IEnumerable<DependencyObject> VisualDepthFirstTraversal(DependencyObject node)
            {
                if (node is null) throw new ArgumentNullException(nameof(node));

                yield return node;

                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(node); i++)
                {
                    var child = VisualTreeHelper.GetChild(node, i);
                    foreach (var descendant in VisualDepthFirstTraversal(child))
                    {
                        yield return descendant;
                    }
                }
            }

            return dialogHost;
        }

        private static DialogHost GetOwningDialogHost(DependencyObject childDependencyObject)
        {
            if (childDependencyObject is null) throw new ArgumentNullException(nameof(childDependencyObject));

            DialogHost? dialogHost = GetVisualAncestry(childDependencyObject).OfType<DialogHost>().FirstOrDefault();

            if (dialogHost is null)
                throw new InvalidOperationException("Unable to find a DialogHost in visual tree ancestory");

            static IEnumerable<DependencyObject> GetVisualAncestry(DependencyObject? leaf)
            {
                while (leaf is not null)
                {
                    yield return leaf;
                    leaf = leaf is Visual || leaf is Visual3D
                        ? VisualTreeHelper.GetParent(leaf)
                        : LogicalTreeHelper.GetParent(leaf);
                }
            }

            return dialogHost;
        }
    }
}