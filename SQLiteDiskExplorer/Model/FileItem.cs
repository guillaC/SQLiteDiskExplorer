namespace SQLiteDiskExplorer.Model
{
    public class FileItem
    {
        public FileInfo FileInfo { get; set; }
        public SQLiteFileHeader? FileHeader { get; set; }
        public Dictionary<string, List<string>>? ColumnKeywordPresence { get; set; }

        public FileItem(FileInfo fileInfo, SQLiteFileHeader? fileHeader, Dictionary<string, List<string>>? columnKeywordIsPresence = null)
        {
            FileInfo = fileInfo;
            FileHeader = fileHeader;
            ColumnKeywordPresence = columnKeywordIsPresence;
        }
    }
}
