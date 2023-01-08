using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Template2.Infrastructure.Oracle
{
    //// 【OracleParameterの注意点】
    //// SQL文中のパラメータ記載方法 => :+文字列
    //// Prameter生成方法 => new OracleParameter("文字列", 置き換えする値)

    /// <summary>
    /// Oracle Data Provider for .NET(ODP.NET) Managed Driverを利用してOracleDBに接続するDaoクラス
    /// </summary>
    internal class OracleOdpDao
    {
        private OracleConnection _connection;
        private OracleTransaction _transaction;

        /// <summary>
        /// トランザクションモード 
        /// Auto:ビギントランス・コミットを自動　Manual:ビギントランス・コミットを手動
        /// </summary>
        private TransactionMode TransactionMode { get; set; } = TransactionMode.Auto;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="user"></param>
        /// <param name="password"></param>
        /// <param name="dataSource"></param>
        /// <exception cref="Exception"></exception>
        internal OracleOdpDao(string user, string password, string dataSource)
        {
            string connectionString = "User Id=" + user + ";Password=" + password + ";Data Source=" + dataSource;
            _connection = new OracleConnection(connectionString);

            try
            {
                _connection.Open();
            }
            catch (Exception e)
            {
                throw new Exception(e.ToString());
            }
        }
        internal void Close()
        {
            _connection.Close();
            _connection.Dispose();
            _transaction.Dispose();
        }

        internal void BeginTransaction()
        {
            _transaction = _connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
        }

        internal void Commit()
        {
            _transaction.Commit();
        }

        internal void Rollback()
        {
            _transaction.Rollback();
        }

        internal void ExecuteNonQuery(string sql)
        {
            OracleCommand command = new OracleCommand(sql, _connection);
            command.ExecuteNonQuery();
        }

        internal OracleDataAdapter ExecuteQueryAdapter(string sql)
        {
            OracleDataAdapter adapter = new OracleDataAdapter(sql, _connection);
            return adapter;
        }

        internal OracleDataReader ExecuteQueryReader(string sql)
        {
            OracleCommand command = new OracleCommand(sql, _connection);
            OracleDataReader reader = command.ExecuteReader();

            return reader;
        }

        internal IReadOnlyList<T> Query<T>(
            string sql,
            Func<OracleDataReader, T> createEntity)
        {
            return Query<T>(sql, null, createEntity);
        }

        internal IReadOnlyList<T> Query<T>(
            string sql,
            OracleParameter[]? parameters,
            Func<OracleDataReader, T> createEntity)
        {
            var result = new List<T>();

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
            return result.AsReadOnly();
        }

        internal T QuerySingle<T>(
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
        internal T QuerySingle<T>(
            string sql,
            OracleParameter[]? parameters,
            Func<OracleDataReader, T> createEntity,
            T nullEntity)
        {
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
            return nullEntity;
        }

        internal void Execute(
            string insert,
            string update,
            OracleParameter[] parameters)
        {
            using (var command = new OracleCommand(update, _connection))
            {
                if (TransactionMode == TransactionMode.Auto)
                {
                    _transaction = _connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                    command.Transaction = _transaction;
                }

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

                    if (TransactionMode == TransactionMode.Auto)
                    {
                        _transaction.Commit();
                    }
                }
                catch(DataException ex)
                {
                    if (TransactionMode == TransactionMode.Auto)
                    {
                        _transaction.Rollback();
                    }
                    _transaction.Dispose();

                    Console.WriteLine(ex.ToString());
                }
            }
        }

        internal void Execute(
            string sql,
            OracleParameter[] parameters)
        {
            using (var command = new OracleCommand(sql, _connection))
            {

                if (TransactionMode == TransactionMode.Auto)
                {
                    _transaction = _connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                    command.Transaction = _transaction;
                }

                try
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    command.ExecuteNonQuery();

                    if (TransactionMode == TransactionMode.Auto)
                    {
                        _transaction.Commit();
                    }
                }
                catch (DataException ex)
                {
                    if (TransactionMode == TransactionMode.Auto)
                    {
                        _transaction.Rollback();
                    }
                    _transaction.Dispose();
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }

    enum TransactionMode
    {
        Auto,
        Manual
    }
}
