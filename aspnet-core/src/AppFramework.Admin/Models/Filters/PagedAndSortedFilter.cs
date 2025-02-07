using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Admin.Models
{
    public class PagedAndSortedFilter : PagedFilter
    {
        public string Sorting { get; set; }
    }
}
