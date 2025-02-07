using System.Collections.Generic;
using AppFramework.Auditing.Dto;
using AppFramework.Dto;

namespace AppFramework.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
