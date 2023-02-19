using System.Data;
using Template2.Domain.Entities;
using Template2.Domain.Repositories;

namespace Template2.Infrastruture.Excel
{
    public class TaskMstExcel : ITaskMstExcelRepository
    {
        public DataTable? GetExcelSheetDataToDataTable(string filePath, string sheetName, bool isFirstRowHeader)
        {
            return ExcelHelper.GetExcelSheetDataToDataTable(filePath, sheetName, isFirstRowHeader);
        }

        public IReadOnlyList<TaskMstEntity> GetExcelSheetDataToList(string filePath, string sheetName, bool isFirstRowHeader)
        {
            return ExcelHelper.GetExcelSheetDataToList(filePath,
                sheetName,
                isFirstRowHeader,
                row =>
                {
                    return new TaskMstEntity(
                        Convert.ToInt32(row[0]),
                        Convert.ToString(row[1]),
                        Convert.ToDateTime(row[2] != DBNull.Value ? row[2] : null));
                });
        }
    }
}
