/*
 * https://github.com/EPPlusSoftware/EPPlus
 * From version 5 EPPlus changes the licence model using a dual license, Polyform Non Commercial/Commercial license.
 * This applies to EPPlus version 5 and later.
 * Previous versions are still licensed LGPL
 *
 * If you use the commercial version of EPPlus, please take the code below.
 */
/*
using System;
using System.Collections.Generic;
using OfficeOpenXml;
using System.IO;

namespace AppFramework.DataExporting.Excel.EpPlus
{
    public abstract class EpPlusExcelImporterBase<TEntity>
    {
        public List<TEntity> ProcessExcelFile(byte[] fileBytes, Func<ExcelWorksheet, int, TEntity> processExcelRow)
        {
            var entities = new List<TEntity>();

            using (var stream = new MemoryStream(fileBytes))
            {
                using (var excelPackage = new ExcelPackage(stream))
                {
                    foreach (var worksheet in excelPackage.Workbook.Worksheets)
                    {
                        var entitiesInWorksheet = ProcessWorksheet(worksheet, processExcelRow);

                        entities.AddRange(entitiesInWorksheet);
                    }
                }
            }

            return entities;
        }

        private List<TEntity> ProcessWorksheet(ExcelWorksheet worksheet, Func<ExcelWorksheet, int, TEntity> processExcelRow)
        {
            var entities = new List<TEntity>();

            for (var i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
            {
                try
                {
                    var entity = processExcelRow(worksheet, i);

                    if (entity != null)
                    {
                        entities.Add(entity);
                    }
                }
                catch (Exception)
                {
                    //ignore
                }
            }

            return entities;
        }
    }
}
*/
