using System.Text.Json;

namespace SQLiteDiskExplorer.Utils
{

    public static class ConfigurationManager
    {
        const string PATH = ".\\Res\\Config.json";

        public static Configuration LoadConfiguration()
        {
            string json = File.ReadAllText(PATH);
            return JsonSerializer.Deserialize<Configuration>(json);
        }

        public static void SaveConfiguration(Configuration config)
        {
            string json = JsonSerializer.Serialize(config, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(PATH, json);
        }

        [Serializable]
        public struct Configuration
        {
            public bool IgnoreInaccessible { get; set; }
            public bool RecurseSubdirectories { get; set; }
            public bool CheckPathKeywordPresence { get; set; }
            public bool CheckColumnKeywordPresence { get; set; }
            public bool CopyToTempIfOpnInAnotherProcess { get; set; }
            public string[] ImportantKeywords { get; set; }
        }
    }
}
