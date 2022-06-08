using AppFramework.Auditing.Dto; 
using Prism.Services.Dialogs;
 
namespace AppFramework.ViewModels
{
    public class AuditLogsDetailsViewModel : HostDialogViewModel
    {
        private AuditLogListDto auditLog;

        public AuditLogListDto AuditLog
        {
            get { return auditLog; }
            set { auditLog = value; RaisePropertyChanged(); }
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                AuditLog = parameters.GetValue<AuditLogListDto>("Value");
            }
        }
    }
}
