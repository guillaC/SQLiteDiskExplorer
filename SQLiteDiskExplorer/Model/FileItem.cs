namespace SQLiteDiskExplorer.Model
{
    public class FileItem
    {
        public FileInfo FileInfo { get; set; }
        public SQLiteFileHeader? FileHeader { get; set; }
        public string TempPath { get; set; } = string.Empty;
        public Dictionary<string, List<string>>? ColumnKeywordPresence { get; set; } // KeyWord, full column name

        public FileItem(FileInfo fileInfo, SQLiteFileHeader? fileHeader, string tempPath = "", Dictionary<string, List<string>>? columnKeywordIsPresence = null)
        {
            FileInfo = fileInfo;
            FileHeader = fileHeader;
            TempPath = tempPath;
            ColumnKeywordPresence = columnKeywordIsPresence;
        }
    }
}
