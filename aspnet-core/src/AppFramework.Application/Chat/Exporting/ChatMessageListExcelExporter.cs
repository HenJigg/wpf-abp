using System;
using System.Collections.Generic;
using System.Linq;
using Abp;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using AppFramework.Chat.Dto;
using AppFramework.DataExporting.Excel.NPOI;
using AppFramework.Dto;
using AppFramework.Storage;

namespace AppFramework.Chat.Exporting
{
    public class ChatMessageListExcelExporter : NpoiExcelExporterBase, IChatMessageListExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ChatMessageListExcelExporter(
            ITempFileCacheManager tempFileCacheManager,
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession
            ) : base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages)
        {
            var tenancyName = messages.Count > 0 ? messages.First().TargetTenantName : L("Anonymous");
            var userName = messages.Count > 0 ? messages.First().TargetUserName : L("Anonymous");

            return CreateExcelPackage(
                $"Chat_{tenancyName}_{userName}.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Messages"));

                    AddHeader(
                        sheet,
                        L("ChatMessage_From"),
                        L("ChatMessage_To"),
                        L("Message"),
                        L("ReadState"),
                        L("CreationTime")
                    );

                    AddObjects(
                        sheet, messages,
                        _ => _.Side == ChatSide.Receiver ? (_.TargetTenantName + "/" + _.TargetUserName) : L("You"),
                        _ => _.Side == ChatSide.Receiver ? L("You") : (_.TargetTenantName + "/" + _.TargetUserName),
                        _ => _.Message,
                        _ => _.Side == ChatSide.Receiver ? _.ReadState : _.ReceiverReadState,
                        _ => _timeZoneConverter.Convert(_.CreationTime, user.TenantId, user.UserId)
                    );

                    for (var i = 1; i <= messages.Count; i++)
                    {
                        //Formatting cells
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd hh:mm:ss");
                    }
                    
                    for (var i = 0; i < 5; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }
    }
}
