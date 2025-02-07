using System.Collections.Generic;
using Abp;
using AppFramework.Chat.Dto;
using AppFramework.Dto;

namespace AppFramework.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
