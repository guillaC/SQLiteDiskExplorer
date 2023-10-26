namespace SQLiteDiskExplorer.Model.Schema
{
    public class Table
    {
        public required string TableName { get; set; }
        public required List<Column> Columns { get; set; }
    }
}
