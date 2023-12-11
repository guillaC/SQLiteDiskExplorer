using SQLiteDiskExplorer.Model;
using System.Text.Json;

namespace SQLiteDiskExplorer.Utils
{
    public static class ConfigurationManager
    {
        const string PATH = ".\\Res\\Config.json";

        public static AppConfig LoadConfiguration()
        {
            AppConfig? tempConfig;
            if (!File.Exists(PATH))
            {
                tempConfig = new()
                {
                    CheckColumnKeywordPresence = true,
                    CheckPathKeywordPresence = true,
                    IgnoreInaccessible = true,
                    RecurseSubdirectories = true,
                    ImportantKeywords = new()
                };
                SaveConfiguration(tempConfig);
                return tempConfig;
            }

            string json = File.ReadAllText(PATH);
            AppConfig result = JsonSerializer.Deserialize<AppConfig>(json)!;
            result.ImportantKeywords = result.ImportantKeywords.ConvertAll(d => d.ToLower());
            return result;
        }

        public static void SaveConfiguration(AppConfig config)
        {
            string json = JsonSerializer.Serialize(config, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(PATH, json);
        }
    }
}
