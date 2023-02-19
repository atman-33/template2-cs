
namespace Template2.Infrastruture.Csv
{
    internal static class CsvHelper
    {
        internal static IReadOnlyList<T> ReadLines<T>(
            string path,
            int startDataRow,
            Func<string[], T> createEntity)
        {
            var list = new List<T>();

            using (StreamReader sr = new StreamReader(path)) 
            {
                try
                {
                    int row = 1;
                    while (!sr.EndOfStream)
                    {
                        //// CSVファイルの一行を読み込む
                        string? line = sr.ReadLine();

                        //// データ読み込み開始行に達していない場合は次のループへ
                        if (row++ < startDataRow)
                        {
                            continue;
                        }

                        //// 読み込んだ一行をカンマ毎に分けて配列に格納する
                        if (line != null)
                        {
                            string[]? values = line.Split(',');
                            var entity = createEntity(values);
                            list.Add(entity);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return list.AsReadOnly();
        }
    }
}
