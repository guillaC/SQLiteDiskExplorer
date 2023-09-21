using Microsoft.Data.Sqlite;
using SQLiteDiskExplorer.Model.Schema;
using System.Data;

namespace SQLiteDiskExplorer.Core
{
    public class SQLiteReader
    {
        private readonly SqliteConnection Connection;
        public List<Table> Schema = new();

        public SQLiteReader(SqliteConnection file)
        {
            Connection = new SqliteConnection($"Data Source={file};Version=3;");
            LoadTableStructure();
        }

        public List<Table> LoadTableStructure()
        {
            Connection.Open();
            DataTable table = Connection.GetSchema("Tables");
            List<Table> tables = new();

            foreach (DataRow row in table.Rows)
            {
                string tableName = row["TABLE_NAME"]?.ToString() ?? "";
                if (!string.IsNullOrWhiteSpace(tableName))
                {
                    List<Column> columns = GetTableColumns(tableName);
                    Table dbTable = new()
                    {
                        TableName = tableName,
                        Columns = columns
                    };
                    tables.Add(dbTable);
                }
            }

            Connection.Close();
            return tables;
        }

        private List<Column> GetTableColumns(string tableName)
        {
            DataTable schemaTable = Connection.GetSchema("Columns", new[] { null, null, tableName });
            List<Column> columns = new List<Column>();

            foreach (DataRow schemaRow in schemaTable.Rows)
            {
                string columnName = schemaRow["COLUMN_NAME"]?.ToString() ?? "";
                if (!string.IsNullOrWhiteSpace(columnName))
                {
                    Column column = new Column
                    {
                        Name = columnName,
                        DataType = schemaRow["DATA_TYPE"]?.ToString() ?? "",
                        Constraint = schemaRow["COLUMN_KEY"]?.ToString() ?? "",
                        IsPrimary = schemaRow["COLUMN_KEY"]?.ToString()?.Equals("PRI", StringComparison.OrdinalIgnoreCase) ?? false
                    };

                    columns.Add(column);
                }
            }

            return columns;
        }
    }
}
