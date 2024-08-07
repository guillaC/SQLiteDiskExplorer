﻿using SQLiteDiskExplorer.Model.Schema;
using System.Data;
using System.Data.SQLite;

namespace SQLiteDiskExplorer.Core
{
    public class SQLiteReader
    {
        private readonly SQLiteConnection Connection;
        public List<Table> Schema = new();
        public byte[] Data;


        public SQLiteReader(string fileName)
        {
            Console.WriteLine($"READING {fileName}");
            Connection = new SQLiteConnection($"Data Source={fileName};");
            
        }

        public void LoadTableStructure()
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
                        tables.Add(new()
                        {
                            TableName = tableName,
                            Columns = GetTableColumns(tableName)
                        });
                    }
                }

                Schema.AddRange(tables);
            }
            catch (Exception ex)
            {
                Console.WriteLine("LoadTableStructure: " + ex.Message);
            }
            finally
            {
                Connection.Close();
            }
        }

        private List<Column> GetTableColumns(string tableName)
        {
            List<Column> columns = new();

            try
            {
                DataTable schemaTable = Connection.GetSchema("Columns", new[] { null, null, tableName });
                foreach (DataRow schemaRow in schemaTable.Rows)
                {
                    string columnName = schemaRow["COLUMN_NAME"]?.ToString() ?? "";

                    if (string.IsNullOrWhiteSpace(columnName)) continue;

                    columns.Add(new()
                    {
                        Name = columnName,
                        DataType = schemaRow["DATA_TYPE"]?.ToString() ?? "",
                        IsPrimary = schemaRow["PRIMARY_KEY"] != DBNull.Value && (bool)schemaRow["PRIMARY_KEY"]
                    });


                    /* TODO : relations entre les tables
                    if (schemaRow["CONSTRAINT_NAME"] != DBNull.Value && schemaRow["CONSTRAINT_TYPE"]?.ToString() == "FOREIGN KEY")
                    {
                        ForeignKeyInfo foreignKey = new()
                        {
                            ReferencedTable = schemaRow["FKEY_TO_TABLE"]?.ToString() ?? "",
                            ReferencedColumn = schemaRow["FKEY_TO_COLUMN"]?.ToString() ?? ""
                        };

                        column.ForeignKey = foreignKey;
                    }
                    */


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetTableColumns({tableName}): {ex.Message}");
            }

            return columns;
        }

        public List<DataRow> GetTableData(Table table)
        {
            List<DataRow> tableData = new();

            try
            {
                Connection.Open();
                using SQLiteCommand command = new($"SELECT * FROM {table.TableName}", Connection);
                using SQLiteDataAdapter adapter = new(command);
                DataTable dataTable = new();
                adapter.Fill(dataTable);
                tableData.AddRange(from DataRow row in dataTable.Rows select row);

            }
            catch (Exception ex)
            {
                Console.WriteLine("GetTableData: " + ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return tableData;
        }

        /*
        private static void WritlnTables(List<Table> tables)
        {
            foreach (var table in tables)
            {
                string columnNames = string.Join(", ", table.Columns.Select(c => c.Name));
                Console.WriteLine($"Table Name: {table.TableName}, Columns: {columnNames}");
            }
        }
        */
    }
}
