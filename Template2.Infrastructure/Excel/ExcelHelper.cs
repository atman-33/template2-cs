using ExcelDataReader;
using System.Data;
using System.Text;

namespace Template2.Infrastruture.Excel
{
    internal static class ExcelHelper
    {
        internal static IReadOnlyList<T> GetExcelSheetDataToList<T>(
            string filePath,
            string sheetName,
            bool isFirstRowColumnName,
            Func<DataRow, T> createEntity)
        {
            var table = ExcelHelper.GetExcelSheetDataToDataTable(filePath, sheetName, isFirstRowColumnName);
            var list = new List<T>();

            foreach(DataRow row in table.Rows)
            {
                var entity = createEntity(row);

                list.Add(entity);
            }

            return list.AsReadOnly();
        }

        internal static DataTable? GetExcelSheetDataToDataTable(string filePath, string sheetName, bool isFirstRowHeader)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var streamReader = new StreamReader(stream))
            {
                IExcelDataReader reader;
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                if (filePath.EndsWith(".xls") || filePath.EndsWith(".xlsx") || filePath.EndsWith(".xlsb"))
                {
                    reader = ExcelReaderFactory.CreateReader(streamReader.BaseStream, new ExcelReaderConfiguration()
                    {
                        FallbackEncoding = Encoding.GetEncoding("Shift_JIS")
                    });
                }
                else if (filePath.EndsWith(".csv"))
                {
                    reader = ExcelReaderFactory.CreateCsvReader(streamReader.BaseStream, new ExcelReaderConfiguration()
                    {
                        FallbackEncoding = Encoding.GetEncoding("Shift_JIS")
                    });
                }
                else
                {
                    Console.WriteLine("ExcelHelper.GetExcelDataBySheetName => サポート対象外の拡張子です。");
                    return null;
                }

                var dataset = reader.AsDataSet();
                var worksheet = dataset.Tables[sheetName];
                reader.Close();

                //// 先頭行がカラム名では無い場合、worksheetを返す
                if (isFirstRowHeader == false)
                {
                    return worksheet;
                }

                //// 以降、先頭行がカラム名の場合

                var table = new DataTable();

                for (int columnIndex = 0; columnIndex < worksheet.Columns.Count; columnIndex++)
                {
                    //// カラム名
                    string columnName = worksheet.Rows[0][columnIndex].ToString();
                    table.Columns.Add(columnName);
                }

                //// 先頭行はカラムのため、rowIndexは1（2行目）から開始
                for (int rowIndex = 1; rowIndex < worksheet.Rows.Count; rowIndex++)
                {
                    var row = table.NewRow();
                 
                    for (int columnIndex = 0; columnIndex < worksheet.Columns.Count; columnIndex++)
                    {
                        row[columnIndex] = worksheet.Rows[rowIndex][columnIndex];
                    }

                    table.Rows.Add(row);
                }

                return table;
            }
        }
    }
}
