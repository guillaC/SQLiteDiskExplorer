using SQLiteDiskExplorer.Model.Schema;
using System.Data;
using System.Data.SQLite;


namespace SQLiteDiskExplorer.Core
{
    public class SQLiteReader
    {
        private readonly SQLiteConnection Connection;
        public List<Table> Schema = new();

        public SQLiteReader(string fileName)
        {
            Console.WriteLine($"READING {fileName}");
            Connection = new SQLiteConnection($"Data Source={fileName}");
            LoadTableStructure();
            WritlnTables(Schema);
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
                    Column column = new()
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

        public static void WritlnTables(List<Table> tables)
        {
            foreach (var table in tables)
            {
                string columnNames = string.Join(", ", table.Columns.Select(c => c.Name));
                Console.WriteLine($"Table Name: {table.TableName}, Columns: {columnNames}");
            }
        }
    }
}
