using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.Localization;
using Abp.Localization.Sources;
using AppFramework.Authorization.Users.Importing.Dto;
using AppFramework.DataExporting.Excel.NPOI;
using NPOI.SS.UserModel;

namespace AppFramework.Authorization.Users.Importing
{
    public class UserListExcelDataReader : NpoiExcelImporterBase<ImportUserDto>, IUserListExcelDataReader
    {
        private readonly ILocalizationSource _localizationSource;

        public UserListExcelDataReader(ILocalizationManager localizationManager)
        {
            _localizationSource = localizationManager.GetSource(AppFrameworkConsts.LocalizationSourceName);
        }

        public List<ImportUserDto> GetUsersFromExcel(byte[] fileBytes)
        {
            return ProcessExcelFile(fileBytes, ProcessExcelRow);
        }

        private ImportUserDto ProcessExcelRow(ISheet worksheet, int row)
        {
            if (IsRowEmpty(worksheet, row))
            {
                return null;
            }

            var exceptionMessage = new StringBuilder();
            var user = new ImportUserDto();

            try
            {
                user.UserName = GetRequiredValueFromRowOrNull(worksheet, row, 0, nameof(user.UserName), exceptionMessage);
                user.Name = GetRequiredValueFromRowOrNull(worksheet, row, 1, nameof(user.Name), exceptionMessage);
                user.Surname = GetRequiredValueFromRowOrNull(worksheet, row, 2, nameof(user.Surname), exceptionMessage);
                user.EmailAddress = GetRequiredValueFromRowOrNull(worksheet, row, 3, nameof(user.EmailAddress), exceptionMessage);
                user.PhoneNumber = GetOptionalValueFromRowOrNull(worksheet, row, 4, exceptionMessage, CellType.String);
                user.Password = GetRequiredValueFromRowOrNull(worksheet, row, 5, nameof(user.Password), exceptionMessage);
                user.AssignedRoleNames = GetAssignedRoleNamesFromRow(worksheet, row, 6);
            }
            catch (System.Exception exception)
            {
                user.Exception = exception.Message;
            }

            return user;
        }

        private string GetRequiredValueFromRowOrNull(
            ISheet worksheet, 
            int row,
            int column,
            string columnName,
            StringBuilder exceptionMessage,
            CellType? cellType = null)
        {
            var cell = worksheet.GetRow(row).GetCell(column);

            if (cellType.HasValue)
            {
                cell.SetCellType(cellType.Value);
            }
            
            var cellValue = cell.StringCellValue;
            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue))
            {
                return cellValue;
            }

            exceptionMessage.Append(GetLocalizedExceptionMessagePart(columnName));
            return null;
        }

        private string GetOptionalValueFromRowOrNull(ISheet worksheet, int row, int column, StringBuilder exceptionMessage, CellType? cellType = null)
        {
            var cell = worksheet.GetRow(row).GetCell(column);
            if (cell == null)
            {
                return string.Empty;
            }

            if (cellType != null)
            {
                cell.SetCellType(cellType.Value);
            }
            
            var cellValue = worksheet.GetRow(row).GetCell(column).StringCellValue;
            if (cellValue != null && !string.IsNullOrWhiteSpace(cellValue))
            {
                return cellValue;
            }
            
            return String.Empty;
        }
        
        private string[] GetAssignedRoleNamesFromRow(ISheet worksheet, int row, int column)
        {
            var cellValue = worksheet.GetRow(row).GetCell(column).StringCellValue;
            if (cellValue == null || string.IsNullOrWhiteSpace(cellValue))
            {
                return new string[0];
            }

            return cellValue.ToString().Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).ToArray();
        }

        private string GetLocalizedExceptionMessagePart(string parameter)
        {
            return _localizationSource.GetString("{0}IsInvalid", _localizationSource.GetString(parameter)) + "; ";
        }

        private bool IsRowEmpty(ISheet worksheet, int row)
        {
            var cell = worksheet.GetRow(row)?.Cells.FirstOrDefault();
            return cell == null || string.IsNullOrWhiteSpace(cell.StringCellValue);
        }
    }
}
