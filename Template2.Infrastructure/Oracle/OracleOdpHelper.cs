using Oracle.ManagedDataAccess.Client;
using System.Data;
using Template2.Domain;

namespace Template2.Infrastructure.Oracle
{
    //// 【OracleParameterの注意点】
    //// SQL文中のパラメータ記載方法 => :+文字列
    //// Prameter生成方法 => new OracleParameter("文字列", 置き換えする値)

    /// <summary>
    /// Oracle Data Provider for .NET(ODP.NET) Managed Driverを利用してOracleDBに接続するヘルパークラス
    /// </summary>
    internal static class OracleOdpHelper
    {
        private static OracleConnection? _connection;
        private static OracleTransaction? _transaction;

        internal static string? User { get; set; } = Shared.OracleUser;
        internal static string? Password { get; set; } = Shared.OraclePassword;
        internal static string? DataSource { get; set; } = Shared.OracleDataSource;

        internal static IReadOnlyList<T> Query<T>(
            string sql,
            Func<OracleDataReader, T> createEntity)
        {
            return Query<T>(sql, null, createEntity);
        }

        internal static IReadOnlyList<T> Query<T>(
            string sql,
            OracleParameter[]? parameters,
            Func<OracleDataReader, T> createEntity)
        {
            var result = new List<T>();

            Open();
            using (var command = new OracleCommand(sql, _connection))
            {
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
            Close();
            return result.AsReadOnly();
        }

        internal static T QuerySingle<T>(
            string sql,
            Func<OracleDataReader, T> createEntity,
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
            OracleParameter[]? parameters,
            Func<OracleDataReader, T> createEntity,
            T nullEntity)
        {
            Open();
            using (var command = new OracleCommand(sql, _connection))
            {
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
            Close();
            return nullEntity;
        }

        internal static void Execute(
            string insert,
            string update,
            OracleParameter[] parameters)
        {
            if (_connection == null)
            {
                return;
            }

            Open();
            using (var command = new OracleCommand(update, _connection))
            {
                _transaction = _connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                command.Transaction = _transaction;

                command.BindByName = true;
                try
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    if (command.ExecuteNonQuery() < 1)
                    {
                        command.CommandText = insert;
                        command.ExecuteNonQuery();
                    }

                    _transaction.Commit();
                    Close();
                }
                catch(DataException ex)
                {
                    _transaction.Rollback();
                    Close();

                    Console.WriteLine(ex.ToString());
                    throw new DataException(ex.Message, ex);
                }
            }
        }

        internal static void Execute(
            string sql,
            OracleParameter[] parameters)
        {
            if (_connection == null)
            {
                return;
            }

            Open();
            using (var command = new OracleCommand(sql, _connection))
            {
                _transaction = _connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                command.Transaction = _transaction;

                try
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    command.ExecuteNonQuery();

                    _transaction.Commit();
                    Close();
                }
                catch (DataException ex)
                {
                    _transaction.Rollback();
                    Close();

                    Console.WriteLine(ex.ToString());
                    throw new DataException(ex.Message, ex);
                }
            }
        }

        internal static void Open()
        {
            string connectionString = "User Id=" + User + ";Password=" + Password + ";Data Source=" + DataSource;
            _connection = new OracleConnection(connectionString);

            try
            {
                _connection.Open();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);
            }
        }

        private static void Close()
        {
            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
            }

            if (_transaction != null)
            {
                _transaction.Dispose();
            }
        }

        private static void BeginTransaction()
        {
            if (_connection == null)
            {
                return;
            }

            _transaction = _connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        }

        private static void Commit()
        {
            if (_transaction == null)
            {
                return;
            }

            _transaction.Commit();
        }

        private static void Rollback()
        {
            if (_transaction == null)
            {
                return;
            }

            _transaction.Rollback();
        }
    }
}
