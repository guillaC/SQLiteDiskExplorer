using SQLiteDiskExplorer.Model;
using System.Text.Json;

namespace SQLiteDiskExplorer.Utils
{

    public static class ConfigurationManager
    {
        const string PATH = ".\\Res\\Config.json";

        public static AppConfig LoadConfiguration()
        {
            string json = File.ReadAllText(PATH);
            return JsonSerializer.Deserialize<AppConfig>(json);
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
