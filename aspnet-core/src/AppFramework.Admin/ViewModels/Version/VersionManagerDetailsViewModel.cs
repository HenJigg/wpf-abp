using AppFramework.Authorization.Users.Profile;
using AppFramework.Shared;
using AppFramework.Admin.Models;
using AppFramework.Version.Dtos;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.IO;
using System.Threading.Tasks;

namespace AppFramework.Admin.ViewModels.Version
{
    public class VersionManagerDetailsViewModel : HostDialogViewModel
    {
        public VersionManagerDetailsViewModel(ProxyProfileControllerService profileControllerService)
        {
            this.profileControllerService = profileControllerService;

            SelectedFileCommand = new DelegateCommand(SelectedFile);
        }

        private VersionListModel model;

        public VersionListModel Model
        {
            get { return model; }
            set { model = value; OnPropertyChanged(); }
        }

        private string filePath;
        private readonly ProxyProfileControllerService profileControllerService;

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; OnPropertyChanged(); }
        }

        public DelegateCommand SelectedFileCommand { get; private set; }

        private void SelectedFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "压缩文件(*.zip)|*.zip";
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult != null && (bool)dialogResult)
                FilePath = fileDialog.FileName;
        }

        public override async Task Save()
        {
            if (Model.Id == 0 && string.IsNullOrWhiteSpace(FilePath)) return;

            if (!Verify(Model).IsValid) return;

            await SetBusyAsync(async () =>
            {
                MemoryStream? stream = null;
                if (!string.IsNullOrWhiteSpace(FilePath))
                {
                    var fileBytes = File.ReadAllBytes(FilePath);
                    stream = new MemoryStream(fileBytes);
                }

                await profileControllerService.UploadVersionFile(content =>
                 {
                     if (stream != null)
                         content.AddFile("file", stream, "file.zip");

                     if (Model.Id > 0)
                         content.AddString(nameof(CreateOrEditAbpVersionDto.Id), Model.Id.ToString());

                     content.AddString(nameof(CreateOrEditAbpVersionDto.Name), Model.Name);
                     content.AddString(nameof(CreateOrEditAbpVersionDto.Version), Model.Version);
                     content.AddString(nameof(CreateOrEditAbpVersionDto.IsForced), Model.IsForced.ToString());
                     content.AddString(nameof(CreateOrEditAbpVersionDto.IsEnable), Model.IsEnable.ToString());
                     content.AddString(nameof(CreateOrEditAbpVersionDto.MinimumVersion), Model.MinimumVersion.ToString());
                 }).WebAsync(async () =>
                 {
                     stream?.Dispose();
                     stream?.Close();
                     await base.Save();
                 }); 
            });
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            await SetBusyAsync(async () =>
            {
                if (parameters.ContainsKey("Value"))
                {
                    var tenant = parameters.GetValue<AbpVersionDto>("Value");
                    Model = Map<VersionListModel>(tenant);
                }
                else
                    Model = new VersionListModel();

                await Task.CompletedTask;
            });
        }
    }
}
