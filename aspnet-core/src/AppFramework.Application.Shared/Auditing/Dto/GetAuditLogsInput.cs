using System;
using Abp.Extensions;
using Abp.Runtime.Validation;
using AppFramework.Common;
using AppFramework.Dto;

namespace AppFramework.Auditing.Dto
{
    public class GetAuditLogsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string UserName { get; set; }

        public string ServiceName { get; set; }

        public string MethodName { get; set; }

        public string BrowserInfo { get; set; }

        public bool? HasException { get; set; }

        public int? MinExecutionDuration { get; set; }

        public int? MaxExecutionDuration { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "ExecutionTime DESC";
            }

            Sorting = DtoSortingHelper.ReplaceSorting(Sorting, s =>
            {
	            if (s.IndexOf("UserName", StringComparison.OrdinalIgnoreCase) >= 0)
	            {
		            s = "User." + s;
	            }
	            else
	            {
		            s = "AuditLog." + s;
	            }

	            return s;
            });
        }
    }
}
