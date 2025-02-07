using System;
using System.Windows;

namespace AppFramework.Admin.HandyUI
{
    public class DialogOpenedEventArgs : RoutedEventArgs
    {
        public DialogOpenedEventArgs(DialogSession session, RoutedEvent routedEvent)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));

            Session = session;
            RoutedEvent = routedEvent;
        }

        /// <summary>
        /// Allows interation with the current dialog session.
        /// </summary>
        public DialogSession Session { get; }
    }
}