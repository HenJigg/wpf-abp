using Abp.Application.Services.Dto;

namespace AppFramework.Version.Dtos
{
    public class GetAllForLookupTableInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
    }
}