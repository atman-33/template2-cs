using Template2.Domain.Entities;
using Template2.Domain.Repositories;
using Template2.Infrastruture.Csv;

namespace Template2.Infrastructure.Csv
{
    public class TaskMstCsv : ITaskMstCsvRepository
    {
        /// <summary>
        /// データ読み込み開始行
        /// </summary>
        const int StartDataRow = 2;

        public IReadOnlyList<TaskMstEntity> GetData(string filePath)
        {
            return CsvHelper.ReadLines(filePath,
                StartDataRow,
                values =>
                {
                    return new TaskMstEntity(
                        Convert.ToInt32(values[0]),
                        Convert.ToString(values[1]),
                        Convert.ToDateTime(values[2]));
                });
        }
    }
}
