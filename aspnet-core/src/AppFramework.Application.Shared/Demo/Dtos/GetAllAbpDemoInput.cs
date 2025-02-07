using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppFramework.Demo.Dtos
{
    public class GetAllAbpDemoInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }
    }
}
