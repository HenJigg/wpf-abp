using Abp.Application.Services.Dto;
using System;

namespace AppFramework.Version.Dtos
{
    public class GetAllAbpVersionsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

    }
}