using System.Data;

namespace Template2.Domain.Modules.Objects
{
    public class EntitiesDataTable
    {
        private DataTable _dataTable;
        private string _rowName = String.Empty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="rowName">行の見出し</param>
        public EntitiesDataTable(string rowName)
        {
            _dataTable = new DataTable();
            _rowName = rowName;
            _dataTable.Columns.Add(rowName);
        }

        public DataView DataView 
        {
            get
            {
                return _dataTable.DefaultView;
            }
        }

        public void SetColumns<TGridValueType>(List<string> columns)
        {
            foreach (var c in columns)
            {
                _dataTable.Columns.Add(c, typeof(TGridValueType));
            }
        }
    }
}
