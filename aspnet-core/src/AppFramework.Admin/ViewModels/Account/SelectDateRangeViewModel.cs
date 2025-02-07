using AppFramework.Shared;
using Prism.Services.Dialogs;
using System;
using System.Threading.Tasks;

namespace AppFramework.Admin.ViewModels
{
    public class SelectDateRangeViewModel : HostDialogViewModel
    {
        private DateTime? startDate;

        public DateTime? StartDate
        {
            get { return startDate; }
            set { startDate = value; OnPropertyChanged(); }
        }

        private DateTime? endDate;

        public DateTime? EndDate
        {
            get { return endDate; }
            set { endDate = value; OnPropertyChanged(); }
        }

        public override async Task Save()
        {
            if (StartDate == null || EndDate == null) return;

            DialogParameters param = new DialogParameters();
            param.Add("StartDate", StartDate);
            param.Add("EndDate", EndDate);
            base.Save(param);

            await Task.CompletedTask;
        }

        public override void OnDialogOpened(IDialogParameters parameters) { }
    }
}
