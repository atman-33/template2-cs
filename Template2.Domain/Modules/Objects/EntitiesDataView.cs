using System.Data;

namespace Template2.Domain.Modules.Objects
{
    public class EntitiesDataView
    {
        private DataTable _dataTable;
        private string _rowName = String.Empty;

        public EntitiesDataView(string rowName)
        {
            _dataTable = new DataTable();
            _rowName = rowName;
            _dataTable.Columns.Add(rowName);
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
