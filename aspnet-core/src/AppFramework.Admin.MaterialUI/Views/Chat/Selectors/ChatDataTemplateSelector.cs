using AppFramework.Shared.Models.Chat;
using System.Windows;
using System.Windows.Controls;

namespace AppFramework.Admin.MaterialUI
{
    public class ChatDataTemplateSelector : DataTemplateSelector
    {
        public ChatDataTemplateSelector()
        {
            ReceiverImageTemplate=FinResource("ReceiverImageTemplate");
            ReceiverFileTemplate=FinResource("ReceiverFileTemplate");
            ReceiverTextTemplate=FinResource("ReceiverTextTemplate");

            SenderImageTemplate=FinResource("SenderImageTemplate");
            SenderFileTemplate=FinResource("SenderFileTemplate");
            SenderTextTemplate=FinResource("SenderTextTemplate");
        }

        public DataTemplate ReceiverImageTemplate { get; private set; }

        public DataTemplate ReceiverFileTemplate { get; private set; }

        public DataTemplate ReceiverTextTemplate { get; private set; }

        public DataTemplate SenderImageTemplate { get; private set; }

        public DataTemplate SenderFileTemplate { get; private set; }

        public DataTemplate SenderTextTemplate { get; private set; }
           
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null && item is ChatMessageModel chatModel)
            {
                if (chatModel.Side == Chat.ChatSide.Sender)
                {
                    switch (chatModel.MessageType)
                    {
                        case "image": return SenderImageTemplate;
                        case "file": return SenderFileTemplate; 
                        case "text": return SenderTextTemplate;
                    }
                }
                else
                {
                    switch (chatModel.MessageType)
                    {
                        case "image": return ReceiverImageTemplate;
                        case "file": return ReceiverFileTemplate; 
                        case "text": return ReceiverTextTemplate;
                    }
                }
            }

            return base.SelectTemplate(item, container);
        }

        private DataTemplate FinResource(string resourceKey)
        {
            return System.Windows.Application.Current.FindResource(resourceKey) as DataTemplate;
        }

    }
}
