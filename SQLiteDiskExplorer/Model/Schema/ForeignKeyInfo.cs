namespace SQLiteDiskExplorer.Model.Schema
{
    public class ForeignKeyInfo
    {
        public required string ReferencedTable { get; set; }
        public required string ReferencedColumn { get; set; }
    }
}
