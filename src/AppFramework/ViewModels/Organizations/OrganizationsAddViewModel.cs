using AppFramework.Common;
using AppFramework.Common.Models;
using AppFramework.Organizations;
using AppFramework.Organizations.Dto;
using Prism.Services.Dialogs;

namespace AppFramework.ViewModels
{
    public class OrganizationsAddViewModel : HostDialogViewModel
    {
        public OrganizationsAddViewModel(IOrganizationUnitAppService appService)
        {
            this.appService = appService;
        }

        #region 字段/属性

        private long? ParentId;
        private bool IsNewOrganization;
        private OrganizationListModel input;
        private readonly IOrganizationUnitAppService appService;

        public OrganizationListModel Input
        {
            get { return input; }
            set { input = value; RaisePropertyChanged(); }
        }

        #endregion

        protected override async void Save()
        {
            await SetBusyAsync(async () =>
             {
                 if (IsNewOrganization)
                 {
                     await WebRequest.Execute(() => appService.CreateOrganizationUnit(
                        new CreateOrganizationUnitInput()
                        {
                            DisplayName = input.DisplayName,
                            ParentId = ParentId
                        }));
                 }
                 else
                 {
                     await WebRequest.Execute(() => appService.UpdateOrganizationUnit(
                         new UpdateOrganizationUnitInput()
                         {
                             Id = input.Id,
                             DisplayName = input.DisplayName
                         }));
                 }
             });
            base.Save();
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                Input = parameters.GetValue<OrganizationListModel>("Value");
            }
            else
            {
                IsNewOrganization = true;
                Input = new OrganizationListModel();
            }

            if (parameters.ContainsKey("ParentId"))
                ParentId = parameters.GetValue<long>("ParentId");
        }
    }
}