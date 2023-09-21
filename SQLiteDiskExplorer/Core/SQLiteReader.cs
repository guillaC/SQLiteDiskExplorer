using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDiskExplorer.Core
{
    public class SQLiteReader
    {
        private readonly SqliteConnection Connection;
        public Dictionary<string, string> TableStructure = new(); // à remplacer par un objet TableStruc

        public SQLiteReader(SqliteConnection file)
        {
            Connection = new SqliteConnection($"Data Source={file};Version=3;");
            LoadTableStructure();
        }

        private void LoadTableStructure()
        {
            Connection.Open();
            DataTable table = Connection.GetSchema("Tables");

            foreach (DataRow row in table.Rows)
            {
                string tableName = row["TABLE_NAME"]?.ToString() ?? "";
                if (!string.IsNullOrWhiteSpace(tableName))
                {
                    DataTable schemaTable = Connection.GetSchema("Columns", new[] { null, null, tableName });
                    List<string> columns = new();

                    foreach (DataRow schemaRow in schemaTable.Rows)
                    {
                        string columnName = schemaRow["COLUMN_NAME"]?.ToString()??"";
                        if (!string.IsNullOrWhiteSpace(columnName)) columns.Add(columnName);
                    }

                    TableStructure[tableName] = string.Join(", ", columns); // TODO : pas tenable
                }
            }

            Connection.Close();
        }
    }
}
