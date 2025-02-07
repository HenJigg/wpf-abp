namespace AppFramework.Shared.Models
{
    public class VersionListModel : EntityObject
    {
        private string name;
        private string version;
        private bool isenable;
        private bool isforced;
        private string minimumVersion;
          
        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged(); }
        }

        public string MinimumVersion
        {
            get { return minimumVersion; }
            set { minimumVersion = value; RaisePropertyChanged(); }
        }

        public string Version
        {
            get { return version; }
            set { version = value; RaisePropertyChanged(); }
        }

        public string DownloadUrl { get; set; }

        public string ChangelogUrl { get; set; }

        public bool IsEnable
        {
            get { return isenable; }
            set { isenable = value; RaisePropertyChanged(); }
        }

        public bool IsForced
        {
            get { return isforced; }
            set { isforced = value; RaisePropertyChanged(); }
        }
    }
}
