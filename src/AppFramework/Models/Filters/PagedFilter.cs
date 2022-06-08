using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Models
{
    public class PagedFilter : BindableBase
    {
        public PagedFilter()
        {
            MaxResultCount = AppConsts.DefaultPageSize;
        }

        public int MaxResultCount { get; set; }

        public int SkipCount { get; set; }
    }
}
