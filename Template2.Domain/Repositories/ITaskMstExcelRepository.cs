using System.Data;
using Template2.Domain.Entities;

namespace Template2.Domain.Repositories
{
    public interface ITaskMstExcelRepository
    {
        DataTable? GetExcelSheetDataToDataTable(string filePath, string sheetName, bool isFirstRowHeader);

        IReadOnlyList<TaskMstEntity> GetExcelSheetDataToList(string filePath, string sheetName, bool isFirstRowHeader);
    }
}
