using Prism.Mvvm;
using System;

namespace AppFramework.Admin.Models
{
    public class LanguageListModel 
    { 
        public int Id { get; set; }

        public int? TenantId { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Icon { get; set; }

        public bool IsDisabled { get; set; }

        public DateTime CreationTime { get; set; }
    }
}