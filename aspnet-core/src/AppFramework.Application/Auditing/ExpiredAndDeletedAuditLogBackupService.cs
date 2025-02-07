using System;
using System.Collections.Generic;
using System.IO;
using Abp.Auditing;
using AppFramework.Configuration;
using AppFramework.DataExporting.Excel.NPOI;
using AppFramework.Dto;
using AppFramework.Storage;
using NPOI.XSSF.UserModel;

namespace AppFramework.Auditing
{
    public class ExpiredAndDeletedAuditLogBackupService : NpoiExcelExporterBase, IExpiredAndDeletedAuditLogBackupService
    {
        private readonly bool _isBackupEnabled;

        private readonly IAppConfigurationAccessor _configurationAccessor;

        public ExpiredAndDeletedAuditLogBackupService(
            ITempFileCacheManager tempFileCacheManager,
            IAppConfigurationAccessor configurationAccessor
        )
            : base(tempFileCacheManager)
        {
            _configurationAccessor = configurationAccessor;
            _isBackupEnabled =
                _configurationAccessor.Configuration["App:AuditLog:AutoDeleteExpiredLogs:ExcelBackup:IsEnabled"] ==
                true.ToString();
        }

        public bool CanBackup() => _isBackupEnabled;

        public void Backup(List<AuditLog> auditLogs)
        {
            if (auditLogs.Count == 0)
            {
                return;
            }

            CreateExcelPackage(
                "AuditLogBackup_" + DateTime.UtcNow.ToString("yyyy-MM-ddTHH.mm.ss.FFFZ") + ".xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("Users"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("TenantId"),
                        L("UserId"),
                        L("ServiceName"),
                        L("MethodName"),
                        L("Parameters"),
                        L("ReturnValue"),
                        L("ExecutionTime"),
                        L("ExecutionDuration"),
                        L("ClientIpAddress"),
                        L("ClientName"),
                        L("BrowserInfo"),
                        L("Exception"),
                        L("ExceptionMessage"),
                        L("ImpersonatorUserId"),
                        L("ImpersonatorTenantId"),
                        L("CustomData")
                    );

                    AddObjects(
                        sheet, auditLogs,
                        _ => _.TenantId,
                        _ => _.UserId,
                        _ => _.ServiceName,
                        _ => _.MethodName,
                        _ => _.Parameters,
                        _ => _.ReturnValue,
                        _ => _.ExecutionTime,
                        _ => _.ExecutionDuration,
                        _ => _.ClientIpAddress,
                        _ => _.ClientName,
                        _ => _.BrowserInfo,
                        _ => _.Exception,
                        _ => _.ExceptionMessage,
                        _ => _.ImpersonatorUserId,
                        _ => _.ImpersonatorTenantId,
                        _ => _.CustomData
                    );

                    for (var i = 1; i <= auditLogs.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[6], "yyyy-mm-dd");
                    }

                    for (var i = 0; i < 16; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });
        }

        protected override void Save(XSSFWorkbook excelPackage, FileDto file)
        {
            var backupFilePath =
                _configurationAccessor.Configuration["App:AuditLog:AutoDeleteExpiredLogs:ExcelBackup:FilePath"];
            if (string.IsNullOrWhiteSpace(backupFilePath))
            {
                return;
            }

            if (!Directory.Exists(backupFilePath))
            {
                Directory.CreateDirectory(backupFilePath);
            }

            using (FileStream excelFile = new FileStream(Path.Combine(backupFilePath, file.FileName), FileMode.Create,
                       System.IO.FileAccess.Write))
            {
                excelPackage.Write(excelFile);
            }
        }
    }
}