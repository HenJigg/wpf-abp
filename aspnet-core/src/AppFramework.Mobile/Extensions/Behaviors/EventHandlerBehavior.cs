using JetBrains.Annotations;
using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace AppFramework.Shared.Behaviors
{
    [Preserve(AllMembers = true)]
    [ContentProperty("Actions")]
    public sealed class EventHandlerBehavior : BehaviorBase<VisualElement>
    {
        private Delegate _eventHandler;

        public static readonly BindableProperty EventNameProperty = BindableProperty.Create("EventName", typeof(string), typeof(EventHandlerBehavior), null, propertyChanged: OnEventNameChanged);

        public static readonly BindableProperty ActionsProperty = BindableProperty.Create("Actions", typeof(ActionCollection), typeof(EventHandlerBehavior), null);

        private static readonly MethodInfo OnEventMethodInfo = typeof(EventHandlerBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");

        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        public ActionCollection Actions
        {
            get
            {
                var actionCollection = (ActionCollection)GetValue(ActionsProperty);
                if (actionCollection == null)
                {
                    actionCollection = new ActionCollection();
                    SetValue(ActionsProperty, actionCollection);
                }
                return actionCollection;
            }
        }

        protected override void OnAttachedTo(VisualElement bindable)
        {
            base.OnAttachedTo(bindable);
            RegisterEvent(EventName);
        }

        protected override void OnDetachingFrom(VisualElement bindable)
        {
            DeregisterEvent(EventName);
            base.OnDetachingFrom(bindable);
        }

        private void RegisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            var eventInfo = AssociatedObject.GetType().GetRuntimeEvent(EventName);
            if (eventInfo == null)
            {
                throw new ArgumentException(
                    string.Format(
                        "EventHandlerBehavior: Can't register the '{0}' event.",
                        EventName
                    )
                );
            }

            _eventHandler = OnEventMethodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
            eventInfo.AddEventHandler(AssociatedObject, _eventHandler);
        }

        private void DeregisterEvent(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            if (_eventHandler == null)
            {
                return;
            }

            var eventInfo = AssociatedObject.GetType().GetRuntimeEvent(EventName);
            if (eventInfo == null)
            {
                throw new ArgumentException(string.Format("EventHandlerBehavior: Can't de-register the '{0}' event.", EventName));
            }

            eventInfo.RemoveEventHandler(AssociatedObject, _eventHandler);
            _eventHandler = null;
        }

        [UsedImplicitly]
        private void OnEvent(object sender, object eventArgs)
        {
            foreach (var bindable in Actions)
            {
                bindable.BindingContext = BindingContext;
                var action = (IAction)bindable;
                action.Execute(sender, eventArgs);
            }
        }

        private static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = (EventHandlerBehavior)bindable;
            if (behavior.AssociatedObject == null)
            {
                return;
            }

            var oldEventName = (string)oldValue;
            var newEventName = (string)newValue;

            behavior.DeregisterEvent(oldEventName);
            behavior.RegisterEvent(newEventName);
        }
    }
}