using AppFramework.Shared.Models.Chat;
using Xamarin.Forms;

namespace AppFramework.Shared.Views.Selectors
{
    public class MessageDataTemplateSelector : DataTemplateSelector
    {
        #region Constructor

        public MessageDataTemplateSelector()
        {
            this.ReceiverImageTemplate = new DataTemplate(typeof(ReceiverImageTemplate));
            this.ReceiverTextTemplate = new DataTemplate(typeof(ReceiverTextTemplate));
            this.SenderImageTemplate = new DataTemplate(typeof(SenderImageTemplate));
            this.SenderTextTemplate = new DataTemplate(typeof(SenderTextTemplate));
        }

        #endregion

        #region Properties

        public DataTemplate ReceiverImageTemplate { get; set; }

        public DataTemplate ReceiverTextTemplate { get; set; }

        public DataTemplate SenderImageTemplate { get; set; }

        public DataTemplate SenderTextTemplate { get; set; }

        #endregion

        #region Methods

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item != null && item is ChatMessageModel chatModel)
            {
                if (chatModel.Side == Chat.ChatSide.Sender)
                {
                    switch (chatModel.MessageType)
                    {
                        case "image": return SenderImageTemplate;
                        case "file": return SenderTextTemplate;
                        case "link": return SenderTextTemplate;
                        case "text": return SenderTextTemplate;
                    }
                }
                else
                {
                    switch (chatModel.MessageType)
                    {
                        case "image": return ReceiverImageTemplate;
                        case "file": return ReceiverTextTemplate;
                        case "link": return ReceiverTextTemplate;
                        case "text": return ReceiverTextTemplate;
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
