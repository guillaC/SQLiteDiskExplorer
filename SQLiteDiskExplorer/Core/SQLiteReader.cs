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

        private void LoadTableStructure()
        {
            try
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

                Schema.AddRange(tables);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("pb " + ex.Message);
                return;
            }
            finally
            {
                Connection.Close();
            }
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
                    Column column = new();
                    column.Name = columnName;
                    column.DataType = schemaRow["DATA_TYPE"]?.ToString() ?? "";
                    column.IsPrimary = (bool)schemaRow["PRIMARY_KEY"];
                    columns.Add(column);
                }
            }

            return columns;
        }

        private static void WritlnTables(List<Table> tables)
        {
            foreach (var table in tables)
            {
                string columnNames = string.Join(", ", table.Columns.Select(c => c.Name));
                Console.WriteLine($"Table Name: {table.TableName}, Columns: {columnNames}");
            }
        }
    }
}
