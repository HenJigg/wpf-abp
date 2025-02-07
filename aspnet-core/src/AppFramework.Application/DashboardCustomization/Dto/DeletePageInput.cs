using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.DashboardCustomization.Dto
{
    public class DeletePageInput
    {
        public string Id { get; set; }

        public string DashboardName { get; set; } 
        
        public string Application { get; set; }
    }
}
