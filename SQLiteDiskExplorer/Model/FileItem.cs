namespace SQLiteDiskExplorer.Model
{
    public class FileItem
    {
        public FileInfo FileInfo { get; set; }
        public SQLiteFileHeader? FileHeader { get; set; }
        public string TempPath { get; set; } = string.Empty;
        public bool PathKeywordIsPresence { get; set; }
        public bool ColumnKeywordIsPresence { get; set; }

        public FileItem(FileInfo fileInfo, SQLiteFileHeader? fileHeader, string tempPath = "", bool pathKeywordIsPresence = false, bool columnKeywordIsPresence = false)
        {
            FileInfo = fileInfo;
            FileHeader = fileHeader;
            TempPath = tempPath;
            PathKeywordIsPresence = pathKeywordIsPresence;
            ColumnKeywordIsPresence = columnKeywordIsPresence;
        }
    }
}
