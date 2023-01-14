using System.Collections.ObjectModel;
using System.Data;
using static System.Net.Mime.MediaTypeNames;
using Template2.Domain.Exceptions;

namespace Template2.Domain.Modules.Objects
{
    public class EntitiesDataTable<TColumnValueObject, TValueType>
    {
        private DataTable _dataTable;

        private string _idColumnName = String.Empty;

        private Dictionary<string, TColumnValueObject> _columnsDictionary = new Dictionary<string, TColumnValueObject>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="idColumnName">項目列（ID列）の見出し</param>
        public EntitiesDataTable(string idColumnName)
        {
            _dataTable = new DataTable();
            _idColumnName = idColumnName;
            _dataTable.Columns.Add(idColumnName);
        }

        /// <summary>
        /// DataView（DataGrid表示用）
        /// </summary>
        public DataView DataView 
        {
            get
            {
                return _dataTable.DefaultView;
            }
        }

        /// <summary>
        /// カラム名称を設定（文字列を利用）
        /// </summary>
        /// <typeparam name="TGridValueType">DataGridに格納される値の型</typeparam>
        /// <param name="columns">カラム名称</param>
        public void SetColumns(List<string> columns)
        {
            foreach (var c in columns)
            {
                _dataTable.Columns.Add(c, typeof(TValueType));
            }
        }

        /// <summary>
        /// カラム名称を設定（ValueObjectのクラスを利用）
        /// ※DataTableのカラム名称をとコードの関係（ValueObject）を格納しておく。
        /// </summary>
        /// <typeparam name="TGridValueType">DataGridに格納される値の型</typeparam>
        /// <param name="columnsDictionary">辞書型のカラム名称（Key:DataGridに表示する名称,  Value：KeyのValueObject）</param>
        public void SetColumns(Dictionary<string, TColumnValueObject> columnsDictionary) 
        {
            _columnsDictionary = columnsDictionary;

            foreach (var c in columnsDictionary)
            {
                _dataTable.Columns.Add(c.Key, typeof(TValueType));
            }
        }

        /// <summary>
        /// Entityのリストのデータを格納
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="entitiesToLookup"></param>
        /// <param name="getColumn"></param>
        /// <param name="getValue"></param>
        public void SetData<TEntity>(List<TEntity> entities,
            ILookup<string, TEntity> entitiesToLookup,
            Func<TEntity, string> getColumn,
            Func<TEntity, TValueType> getValue)
        {
            var newVarsFromVars = entitiesToLookup.Select(v => new
            {
                ID = v.Key,
                Values = v.ToDictionary(i => getColumn(i), i => getValue(i))
            });

            //// 実際のデータをセット
            foreach (var newVar in newVarsFromVars)
            {
                int idx = 0;
                var row = _dataTable.NewRow();

                //// IDカラムにIDを設定
                row[idx++] = newVar.ID;

                //// カラム名に対応する値が存在する場合はセット
                foreach (var columnName in _dataTable.Columns.Cast<DataColumn>().Skip(1).Select(c => c.ColumnName))
                {
                    row[idx++] = newVar.Values.ContainsKey(columnName) ? newVar.Values[columnName].ToString() : DBNull.Value;
                }

                //// 行データを追加
                _dataTable.Rows.Add(row);
            }
        }

        /// <summary>
        /// DataTableをEntityのObservableCollectionに変換
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="createEntity"></param>
        /// <returns></returns>
        public ObservableCollection<TEntity>? ToEntities<TEntity>(
            Func<string, KeyValuePair<string, string>, TEntity> createEntity)
        {
            if (_dataTable == null)
            {
                return null;
            }

            var entities = new ObservableCollection<TEntity>();

            for (int rowIndex = 0; rowIndex < _dataTable.Rows.Count; rowIndex++)
            {
                //// ID列のデータ
                string idValue = _dataTable.Rows[rowIndex][_idColumnName].ToString();

                var itemValues = new Dictionary<string, string>();

                for (int columnIndex = 0; columnIndex < _dataTable.Columns.Count; columnIndex++)
                {
                    //// カラム名
                    string colunIndexName = _dataTable.Columns[columnIndex].ColumnName;

                    //// 行に対してユニークなIDを示すカラム名は、値を格納せずにループを飛ばす
                    if (colunIndexName == _idColumnName)
                    {
                        continue;
                    }

                    if (_dataTable.Rows[rowIndex].IsNull(columnIndex))
                    {
                        itemValues.Add(colunIndexName, null);
                    }
                    else
                    {
                        itemValues.Add(colunIndexName, _dataTable.Rows[rowIndex][columnIndex].ToString());
                    }
                }

                foreach (var keyValuePair in itemValues)
                {
                    if (keyValuePair.Value == null)
                    {
                        continue;
                    }

                    entities.Add(createEntity(idValue, keyValuePair));
                }
            }

            return entities;
        }

        /// <summary>
        /// DataTableをEntityのObservableCollectionに変換。DataTableのカラム名称をValueObjectにしている場合は、このメソッドを利用する。
        /// ※DataTableのカラム名称をDBに格納する際、コードに変換する場合はValueObjectからデータを取得する。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="createEntity"></param>
        /// <returns></returns>
        public ObservableCollection<TEntity>? ToEntities<TEntity>(
            Func<string, KeyValuePair<string, string>, TColumnValueObject, TEntity> createEntity)
        {
            if (_dataTable == null)
            {
                return null;
            }

            var entities = new ObservableCollection<TEntity>();

            for (int rowIndex = 0; rowIndex < _dataTable.Rows.Count; rowIndex++)
            {
                //// ID列のデータ
                string idValue = _dataTable.Rows[rowIndex][_idColumnName].ToString();

                var itemValues = new Dictionary<string, string>();

                for (int columnIndex = 0; columnIndex < _dataTable.Columns.Count; columnIndex++)
                {
                    //// カラム名
                    string colunIndexName = _dataTable.Columns[columnIndex].ColumnName;

                    //// 行に対してユニークなIDを示すカラム名は、値を格納せずにループを飛ばす
                    if (colunIndexName == _idColumnName)
                    {
                        continue;
                    }

                    if (_dataTable.Rows[rowIndex].IsNull(columnIndex))
                    {
                        itemValues.Add(colunIndexName, null);
                    }
                    else
                    {
                        itemValues.Add(colunIndexName, _dataTable.Rows[rowIndex][columnIndex].ToString());
                    }
                }

                foreach (var keyValuePair in itemValues)
                {
                    if (keyValuePair.Value == null)
                    {
                        continue;
                    }

                    entities.Add(createEntity(idValue, keyValuePair, _columnsDictionary.GetValueOrDefault(keyValuePair.Key)));
                }
            }

            return entities;
        }

        /// <summary>
        /// 表の値がFloatに変換可能か確認する。不可の場合は例外を発生させる。
        /// </summary>
        /// <param name="exceptionMessage"></param>
        /// <exception cref="InputException"></exception>
        public void CanConvertFloat(string exceptionMessage)
        {
            if (_dataTable == null)
            {
                return;
            }

            for (int rowIndex = 0; rowIndex < _dataTable.Rows.Count; rowIndex++)
            {
                //var itemValues = new Dictionary<string, string>();

                for (int columnIndex = 0; columnIndex < _dataTable.Columns.Count; columnIndex++)
                {
                    //// カラム名
                    string colunIndexName = _dataTable.Columns[columnIndex].ColumnName;

                    //// 行に対してユニークなIDを示すカラム名は、ループを飛ばす
                    if (colunIndexName == _idColumnName)
                    {
                        continue;
                    }

                    if (!_dataTable.Rows[rowIndex].IsNull(columnIndex))
                    {
                        var value = _dataTable.Rows[rowIndex][columnIndex].ToString();

                        float floatValue;
                        if (!float.TryParse(value, out floatValue))
                        {
                            throw new InputException(exceptionMessage);
                        }
                    }
                }
            }
        }
    }
}
