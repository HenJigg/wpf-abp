using AppFramework.Shared.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AppFramework.Admin.Models
{
    public partial class VersionListModel : EntityObject
    {
        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public string version;

        [ObservableProperty]
        public bool isEnable;

        [ObservableProperty]
        public bool isForced;

        [ObservableProperty]
        public string minimumVersion;
             
        public string DownloadUrl { get; set; }

        public string ChangelogUrl { get; set; } 
    }
}
