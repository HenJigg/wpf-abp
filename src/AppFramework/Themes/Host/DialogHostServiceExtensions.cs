using AppFramework.Services;
using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace AppFramework.WindowHost
{
    public enum MessageType
    {
        Info,
        Warning,
        Error,
        Question
    }

    public static class DialogHostServiceExtensions
    {
        public static readonly string default_IdentifierName = "Root";

        public static void Info(this IHostDialogService dialogHostService, string message, string IdentifierName = "")
        {
            _ = ShowDialog(dialogHostService, MessageType.Info, message, IdentifierName);
        }

        public static void Warning(this IHostDialogService dialogHostService, string message, string IdentifierName = "")
        {
            _ = ShowDialog(dialogHostService, MessageType.Warning, message, IdentifierName);
        }

        public static void Error(this IHostDialogService dialogHostService, string message, string IdentifierName = "")
        {
            _ = ShowDialog(dialogHostService, MessageType.Error, message, IdentifierName);
        }

        public static async Task<IDialogResult> Question(this IHostDialogService dialogHostService, string message, string IdentifierName = "")
        {
            return await ShowDialog(dialogHostService, MessageType.Question, message);
        }

        internal static async Task<IDialogResult> ShowDialog(
            IHostDialogService dialogHostService,
            MessageType messageType,
            string message
            , string IdentifierName = "")
        {
            if (string.IsNullOrWhiteSpace(IdentifierName))
                IdentifierName = default_IdentifierName;

            DialogParameters param = new DialogParameters();

            param.Add("Type", messageType);
            param.Add("Message", message);

            return await dialogHostService.ShowDialogAsync("MessageView", param, IdentifierName);
        }
    }
}