namespace SQLiteDiskExplorer.Model
{
    [Serializable]
    public class AppConfig
    {
        public bool IgnoreInaccessible { get; set; }
        public bool RecurseSubdirectories { get; set; }
        public bool CheckPathKeywordPresence { get; set; }
        public bool CheckColumnKeywordPresence { get; set; }
        public required List<string> ImportantKeywords { get; set; }
    }
}
