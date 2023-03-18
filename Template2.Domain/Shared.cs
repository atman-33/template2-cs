using System.Configuration;

namespace Template2.Domain
{
    /// <summary>
    /// Shared
    /// </summary>
    public static class Shared
    {
        /// <summary>
        /// Fakeの時Trure（1:Fake）
        /// </summary>
        public static bool IsFake { get; } = (ConfigurationManager.AppSettings["IsFake"] == "1");

        /// <summary>
        /// SQLite 接続子
        /// </summary>
        public static string? SQLiteConnectionString { get; } = ConfigurationManager.AppSettings["SQLiteConnectionString"];

        public static string? OracleUser { get; } = ConfigurationManager.AppSettings["OracleUser"];
        public static string? OraclePassword { get; } = ConfigurationManager.AppSettings["OraclePassword"];
        public static string? OracleDataSource { get; } = ConfigurationManager.AppSettings["OracleDataSource"];

        public static int TimerPeriod { get; } = Convert.ToInt32(ConfigurationManager.AppSettings["TimerPeriod"]);

        /// <summary>
        /// Sample007Viewをアップデートした日時
        /// </summary>
        public static DateTime Sample007ViewUpdatedTime { get; set; }
    }
}