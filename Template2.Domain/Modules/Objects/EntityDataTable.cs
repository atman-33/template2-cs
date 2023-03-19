using System.Collections.ObjectModel;
using System.Data;
using Template2.Domain.Exceptions;

namespace Template2.Domain.Modules.Objects
{
    public class EntityDataTable<TItemHeaderValueObject, TItemValueType>
    {
        private DataTable _dataTable;

        private Dictionary<string, TItemHeaderValueObject> _itemHeaders = new Dictionary<string, TItemHeaderValueObject>();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="idHeader">項目列（ID列）の見出し</param>
        /// <param name="idReadOnly">項目列（ID列）を読取専用とするならtrue</param>
        public EntityDataTable(string idHeader, bool idReadOnly)
        {
            _dataTable = new DataTable();
            
            IdHeader = idHeader;
            _dataTable.Columns.Add(idHeader);

            var dataColumn = _dataTable.Columns[idHeader];
            if (dataColumn != null)
            {
                dataColumn.ReadOnly = idReadOnly;
            }

            //// その他設定メモ
            //// _dataTable.Columns[idColumnName].Unique = true;                        // ユニーク設定
            //// _dataTable.Columns[idColumnName].ColumnMapping = MappingType.Hidden;   // 非表示設定のはずだが、Viewに反映されない
        }

        public DataTable DataTable { get { return _dataTable; } }

        public string IdHeader { get; private set; } = String.Empty;
        public string IdNameHeader { get; private set; } = String.Empty;

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
        /// ID名称の見出しを設定
        /// </summary>
        /// <param name="idNameHeader">ID名称列を読取専用とするならtrue</param>
        public void SetIdNameHeader(string idNameHeader, bool idNameReadOnly)
        {
            _dataTable.Columns.Add(idNameHeader, typeof(string));
            IdNameHeader = idNameHeader;

            var dataColumn = _dataTable.Columns[idNameHeader];
            if (dataColumn != null)
            {
                dataColumn.ReadOnly = idNameReadOnly;
            }
        }

        /// <summary>
        /// アイテム項目のヘッダーを設定（文字列を利用）
        /// </summary>
        /// <typeparam name="TGridValueType">DataGridに格納される値の型</typeparam>
        /// <param name="itemHeaders">カラム名称</param>
        public void SetItemHeaders(List<string> itemHeaders)
        {
            foreach (var header in itemHeaders)
            {
                _dataTable.Columns.Add(header, typeof(TItemValueType));
            }
        }

        /// <summary>
        /// アイテム項目のヘッダーを設定（ValueObjectのクラスを利用）
        /// ※DataTableのカラム名称をとコードの関係（ValueObject）を格納しておく。
        /// </summary>
        /// <typeparam name="TGridValueType">DataGridに格納される値の型</typeparam>
        /// <param name="itemHeaders">辞書型のカラム名称（Key:DataGridに表示する名称,  Value：KeyのValueObject）</param>
        public void SetItemHeaders(Dictionary<string, TItemHeaderValueObject> itemHeaders) 
        {
            _itemHeaders = itemHeaders;

            foreach (var header in itemHeaders)
            {
                _dataTable.Columns.Add(header.Key, typeof(TItemValueType));
            }
        }

        /// <summary>
        /// IDのデータをセット
        /// </summary>
        /// <param name="id"></param>
        public void SetIdData(string id)
        {
            var row = _dataTable.NewRow();

            //// IDカラムにIDを設定
            row[IdHeader] = id;

            //// 行データを追加
            _dataTable.Rows.Add(row);
        }

        /// <summary>
        /// ID名称のデータをセット
        /// </summary>
        /// <param name="id"></param>
        /// <param name="idName"></param>
        public void SetIdData(string id, string idName)
        {
            var row = _dataTable.NewRow();

            //// IDカラムにIDを設定
            row[IdHeader] = id;

            //// ID名称カラムにID名称を設定
            row[IdNameHeader] = idName;

            //// 行データを追加
            _dataTable.Rows.Add(row);
        }

        /// <summary>
        /// IDカラムにデータが格納されている状態で、アイテムカラムのデータをセット
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="entitiesToLookup"></param>
        /// <param name="getColumn"></param>
        /// <param name="getValue"></param>
        public void SetItemData<TEntity>(
            ILookup<string, TEntity> entitiesToLookup,
            Func<TEntity, string> getColumn,
            Func<TEntity, TItemValueType> getValue)
        {
            var newVarsFromVars = entitiesToLookup.Select(v => new
            {
                Id = v.Key,
                Values = v.ToDictionary(i => getColumn(i), i => getValue(i))
            });

            //// 実際のデータをセット
            foreach (var newVar in newVarsFromVars)
            {
                int rowIndex;

                for (rowIndex = 0; rowIndex < _dataTable.Rows.Count; rowIndex++)
                {
                    //// ID列のデータ
                    string? idValue = _dataTable.Rows[rowIndex][IdHeader].ToString();

                    if (newVar.Id == idValue)
                    {
                        break;
                    }
                }

                //// ID列にセットしたデータが見つからない場合は次のループへ
                if (rowIndex >= _dataTable.Rows.Count)
                {
                    continue;
                }
                
                //// カラム名に対応する値が存在する場合はセット
                foreach (var columnName in _dataTable.Columns.Cast<DataColumn>().Skip(1).Select(c => c.ColumnName))
                {
                    if (columnName == IdNameHeader)
                    {
                        continue;
                    }

                    if (!newVar.Values.ContainsKey(columnName))
                    {
                        continue;
                    }

                    var data = newVar.Values[columnName];
                    if (data == null)
                    {
                        _dataTable.Rows[rowIndex][columnName] = DBNull.Value;
                    }
                    else
                    {
                        _dataTable.Rows[rowIndex][columnName] = newVar.Values.ContainsKey(columnName) ? data.ToString() : DBNull.Value;
                    }
                }
            }
        }

        /// <summary>
        /// ID及びアイテムカラムにデータをセット（Entityのリストのデータを格納）
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="entitiesToLookup"></param>
        /// <param name="getColumn"></param>
        /// <param name="getValue"></param>
        public void SetIdItemData<TEntity>(
            ILookup<string, TEntity> entitiesToLookup,
            Func<TEntity, string> getColumn,
            Func<TEntity, TItemValueType> getValue)
        {
            var newVarsFromVars = entitiesToLookup.Select(v => new
            {
                Id = v.Key,
                Values = v.ToDictionary(i => getColumn(i), i => getValue(i))
            });

            //// 実際のデータをセット
            foreach (var newVar in newVarsFromVars)
            {
                int idx = 0;
                var row = _dataTable.NewRow();

                //// IDカラムにIDを設定
                row[idx++] = newVar.Id;

                //// カラム名に対応する値が存在する場合はセット
                foreach (var columnName in _dataTable.Columns.Cast<DataColumn>().Skip(1).Select(c => c.ColumnName))
                {
                    var data = newVar.Values[columnName];
                    if (data == null)
                    {
                        row[idx++] = DBNull.Value;
                    }
                    else
                    {
                        row[idx++] = newVar.Values.ContainsKey(columnName) ? data.ToString() : DBNull.Value;
                    }
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
                string? idValue = _dataTable.Rows[rowIndex][IdHeader].ToString();

                var itemValues = new Dictionary<string, string>();

                for (int columnIndex = 0; columnIndex < _dataTable.Columns.Count; columnIndex++)
                {
                    //// カラム名
                    string colunIndexName = _dataTable.Columns[columnIndex].ColumnName;

                    //// 行に対してユニークなIDを示すカラム名は、値を格納せずにループを飛ばす
                    if (colunIndexName == IdHeader)
                    {
                        continue;
                    }

                    if (_dataTable.Rows[rowIndex].IsNull(columnIndex))
                    {
                        itemValues.Add(colunIndexName, string.Empty);
                    }
                    else
                    {
                        var data = _dataTable.Rows[rowIndex][columnIndex];
                        if (data == null)
                        {
                            itemValues.Add(colunIndexName, string.Empty);
                        }
                        else
                        {
                            itemValues.Add(colunIndexName, data.ToString() ?? string.Empty);
                        }
                    }
                }

                foreach (var keyValuePair in itemValues)
                {
                    if (keyValuePair.Value == null)
                    {
                        continue;
                    }

                    if (idValue != null)
                    {
                        entities.Add(createEntity(idValue, keyValuePair));
                    }
                }
            }

            return entities;
        }

        /// <summary>
        /// 指定した行のデータ合計を返す。
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public float SumRowData(int rowIndex)
        {
            float sum = 0f;

            foreach (var columnName in _dataTable.Columns.Cast<DataColumn>().Skip(1).Select(c => c.ColumnName))
            {
                if (columnName == IdHeader)
                {
                    continue;
                }

                if (columnName == IdNameHeader)
                {
                    continue;
                }

                float floatValue;
                var data = _dataTable.Rows[rowIndex][columnName] as string;

                if (data != null)
                {
                    if (float.TryParse(data, out floatValue))
                    {
                        sum += floatValue;
                    }
                }
            }

            return sum;
        }

        /// <summary>
        /// DataTableをEntityのObservableCollectionに変換。DataTableのカラム名称をValueObjectにしている場合は、このメソッドを利用する。
        /// ※DataTableのカラム名称をDBに格納する際、コードに変換する場合はValueObjectからデータを取得する。
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="createEntity"></param>
        /// <returns></returns>
        public ObservableCollection<TEntity>? ToEntities<TEntity>(
            Func<string, KeyValuePair<string, string>, TItemHeaderValueObject, TEntity> createEntity)
        {
            if (_dataTable == null)
            {
                return null;
            }

            var entities = new ObservableCollection<TEntity>();

            for (int rowIndex = 0; rowIndex < _dataTable.Rows.Count; rowIndex++)
            {
                //// ID列のデータ
                string? idValue = _dataTable.Rows[rowIndex][IdHeader].ToString();

                var itemValues = new Dictionary<string, string>();

                for (int columnIndex = 0; columnIndex < _dataTable.Columns.Count; columnIndex++)
                {
                    //// カラム名
                    string colmunIndexName = _dataTable.Columns[columnIndex].ColumnName;

                    //// IDもしくはID名称を示すカラム名は、ループを飛ばす
                    if (colmunIndexName == IdHeader || colmunIndexName == IdNameHeader)
                    {
                        continue;
                    }

                    if (_dataTable.Rows[rowIndex].IsNull(columnIndex))
                    {
                        itemValues.Add(colmunIndexName, string.Empty);
                    }
                    else
                    {
                        itemValues.Add(colmunIndexName, _dataTable.Rows[rowIndex][columnIndex].ToString() ?? string.Empty);
                    }
                }

                foreach (var keyValuePair in itemValues)
                {
                    if (keyValuePair.Value == null)
                    {
                        continue;
                    }

                    if (keyValuePair.Value == string.Empty)
                    {
                        continue;
                    }

                    if (idValue != null)
                    {
                        if (_itemHeaders != null)
                        {
                            var vo = _itemHeaders.GetValueOrDefault(keyValuePair.Key);
                            if (vo != null)
                            {
                                entities.Add(createEntity(idValue, keyValuePair, vo));
                            }
                        }
                    }
                }
            }

            return entities;
        }

        /// <summary>
        /// 表の値がIntに変換可能か確認する。不可の場合は例外を発生させる。
        /// </summary>
        /// <param name="exceptionMessage"></param>
        /// <exception cref="InputException"></exception>
        public void CanConvertInt(string exceptionMessage)
        {
            if (_dataTable == null)
            {
                return;
            }

            for (int rowIndex = 0; rowIndex < _dataTable.Rows.Count; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < _dataTable.Columns.Count; columnIndex++)
                {
                    //// カラム名
                    string columnIndexName = _dataTable.Columns[columnIndex].ColumnName;

                    //// IDもしくはID名称を示すカラム名は、ループを飛ばす
                    if (columnIndexName == IdHeader || columnIndexName == IdNameHeader)
                    {
                        continue;
                    }

                    if (!_dataTable.Rows[rowIndex].IsNull(columnIndex))
                    {
                        var value = _dataTable.Rows[rowIndex][columnIndex].ToString();

                        int intValue;
                        if (!int.TryParse(value, out intValue))
                        {
                            throw new InputException(exceptionMessage);
                        }
                    }
                }
            }
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
                for (int columnIndex = 0; columnIndex < _dataTable.Columns.Count; columnIndex++)
                {
                    //// カラム名
                    string columnIndexName = _dataTable.Columns[columnIndex].ColumnName;

                    //// IDもしくはID名称を示すカラム名は、ループを飛ばす
                    if (columnIndexName == IdHeader || columnIndexName == IdNameHeader)
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
