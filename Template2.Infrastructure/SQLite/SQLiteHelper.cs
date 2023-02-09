using System.Data.SQLite;
using Template2.Domain;

namespace Template2.Infrastructure.SQLite
{
    //// 【SQLiteParameterの注意点】
    //// SQL文中のパラメータ記載方法 => @+文字列
    //// Prameter生成方法 => new SQLiteParameter("@+文字列", 置き換えする値)

    /// <summary>
    /// SQLiteHelper NuGetパッケージ：System.Data.SQLite.Coreに対応
    /// </summary>
    internal static class SQLiteHelper
    {
        /// <summary>
        /// SQLiteファイルパス
        /// ネットワークフォルダのパスを指定する際は、先頭が「\\\\」から始まる事に注意
        /// </summary>
        internal static string? DataSource { get; set; } = Shared.SQLiteConnectionString;

        internal static void Open()
        {
            var sqlConnectionStringBuilder = new SQLiteConnectionStringBuilder { DataSource = SQLiteHelper.DataSource };
            using (var connection = new SQLiteConnection(sqlConnectionStringBuilder.ToString()))
            using (var command = new SQLiteCommand(connection))
            {
                try
                {
                    connection.Open();      //// SQLiteのDBに接続
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// クエリを実行し、複数レコードを取得（パラメータ変換無し）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="createEntity"></param>
        /// <returns></returns>
        internal static IReadOnlyList<T> Query<T>(
            string sql,
            Func<System.Data.SQLite.SQLiteDataReader, T> createEntity)
        {
            return Query<T>(sql, null, createEntity);
        }

        /// <summary>
        /// クエリを実行し、複数レコードを取得（パラメータ変換有り）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="createEntity"></param>
        /// <returns></returns>
        internal static IReadOnlyList<T> Query<T>(
            string sql,
            SQLiteParameter []? parameters,
            Func<SQLiteDataReader, T> createEntity)
        {
            var result = new List<T>();

            var sqlConnectionStringBuilder = new SQLiteConnectionStringBuilder { DataSource = SQLiteHelper.DataSource };
            using (var connection = new SQLiteConnection(sqlConnectionStringBuilder.ToString()))
            using (var command = new SQLiteCommand(sql, connection))
            {
                connection.Open();      //// SQLiteのDBに接続

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.Add(createEntity(reader));
                    }
                }
            }
            return result.AsReadOnly();
        }

        /// <summary>
        /// クエリを実行し、単体レコードを取得（パラメータ変換無し）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="createEntity"></param>
        /// <param name="nullEntity"></param>
        /// <returns></returns>
        internal static T QuerySingle<T>(
            string sql,
            Func<SQLiteDataReader, T> createEntity,
            T nullEntity)
        {
            return QuerySingle<T>(sql, null, createEntity, nullEntity);
        }

        /// <summary>
        /// クエリを実行し、単体レコードを取得（パラメータ変換有り）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="createEntity"></param>
        /// <param name="nullEntity"></param>
        /// <returns></returns>
        internal static T QuerySingle<T>(
            string sql,
            SQLiteParameter[]? parameters,
            Func<SQLiteDataReader, T> createEntity,
            T nullEntity)
        {
            var sqlConnectionStringBuilder = new SQLiteConnectionStringBuilder { DataSource = SQLiteHelper.DataSource };
            using (var connection = new SQLiteConnection(sqlConnectionStringBuilder.ToString()))
            using (var command = new SQLiteCommand(sql, connection))
            {
                connection.Open();      //// SQLiteのDBに接続

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return createEntity(reader);
                    }
                }
            }
            return nullEntity;
        }

        /// <summary>
        /// SQL実行（updateが実行されない場合はinsertを実行）
        /// </summary>
        /// <param name="insert"></param>
        /// <param name="update"></param>
        /// <param name="parameters"></param>
        internal static void Execute(
            string insert,
            string update,
            SQLiteParameter[] parameters
            )
        {
            var sqlConnectionStringBuilder = new SQLiteConnectionStringBuilder { DataSource = SQLiteHelper.DataSource };
            using (var connection = new SQLiteConnection(sqlConnectionStringBuilder.ToString()))
            using (var command = new SQLiteCommand(update, connection))
            {
                connection.Open();      //// SQLiteのDBに接続

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                if (command.ExecuteNonQuery() < 1)
                {
                    command.CommandText = insert;
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// SQL実行
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        internal static void Execute(
            string sql,
            SQLiteParameter[] parameters
            )
        {
            var sqlConnectionStringBuilder = new SQLiteConnectionStringBuilder { DataSource = SQLiteHelper.DataSource };
            using (var connection = new SQLiteConnection(sqlConnectionStringBuilder.ToString()))
            using (var command = new SQLiteCommand(sql, connection))
            {
                connection.Open();      //// SQLiteのDBに接続

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                command.ExecuteNonQuery();
            }
        }
    }
}
