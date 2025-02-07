using AppFramework.Shared;
using AppFramework.Admin.Models;
using AppFramework.Organizations;
using AppFramework.Organizations.Dto; 
using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace AppFramework.Admin.ViewModels
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
            set { input = value; OnPropertyChanged(); }
        }

        #endregion

        public override async Task Save()
        {
            await SetBusyAsync(async () =>
             {
                 if (IsNewOrganization)
                 {
                     await appService.CreateOrganizationUnit(
                          new CreateOrganizationUnitInput()
                          {
                              DisplayName = input.DisplayName,
                              ParentId = ParentId
                          }).WebAsync();
                 }
                 else
                 {
                     await appService.UpdateOrganizationUnit(
                         new UpdateOrganizationUnitInput()
                         {
                             Id = input.Id,
                             DisplayName = input.DisplayName
                         }).WebAsync();
                 }
             });
            await base.Save();
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