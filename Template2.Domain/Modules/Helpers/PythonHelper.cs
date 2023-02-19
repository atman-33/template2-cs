using System.Diagnostics;

namespace Template2.Domain.Modules.Helpers
{
    public static class PythonHelper
    {
        /// <summary>
        /// プロセスの実行
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IEnumerable<string> PythonCall(string filePath, string args = "")
        {
            if (File.Exists(filePath) == false)
            {
                throw new Exception("Pythonファイルが存在しません。");
            }

            ProcessStartInfo psInfo = new ProcessStartInfo();

            //// 実行するファイルをセット
            psInfo.FileName = "Python";

            //// 引数をセット
            psInfo.Arguments = string.Format("\"{0}\" {1}", filePath, args);

            //// コンソール・ウィンドウを開かない
            psInfo.CreateNoWindow = true;

            //// シェル機能を使用しない
            psInfo.UseShellExecute = false;

            //// 標準出力をリダイレクトする
            psInfo.RedirectStandardOutput = true;

            //// プロセスを開始
            Process p = Process.Start(psInfo);

            //// アプリのコンソール出力結果を全て受け取る
            string line;
            int row = 1;
            while ((line = p.StandardOutput.ReadLine()) != null)
            {
                Debug.WriteLine(line);
                if (row++ == 1)
                {
                    yield return line;
                }
                else
                {
                    yield return Environment.NewLine + line;
                }

            }
        }
    }
}
