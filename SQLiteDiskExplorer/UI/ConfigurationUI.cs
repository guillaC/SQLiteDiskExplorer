using ImGuiNET;
using SQLiteDiskExplorer.Model;
using SQLiteDiskExplorer.Utils;

namespace SQLiteDiskExplorer.UI
{
    public class ConfigurationUI
    {
        bool firstLoad = true;
        bool isOpen = true;
        int selectedLbID;
        string newKeyWord = "";

        AppConfig config;

        public void Show()
        {
            if (!isOpen) return;
            ImGui.Begin("Configuration", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize);

            if (firstLoad)
            {
                config = ConfigurationManager.LoadConfiguration();
                firstLoad = !firstLoad;
            }

            ShowCurrentConfiguration();
            ShowActions();
            ImGui.End();
        }

        private void ShowCurrentConfiguration()
        {
            ImGui.BeginGroup();
            config.RecurseSubdirectories = ShowCheckboxAndGetUpdatedValue(config.RecurseSubdirectories, "Scan Subdirectories");
            config.IgnoreInaccessible = ShowCheckboxAndGetUpdatedValue(config.IgnoreInaccessible, "Ignore Inaccessible Files");
            Front.HelpMarker("Stops analysis when access is denied.");
            config.CopyToTempIfOpnInAnotherProcess = ShowCheckboxAndGetUpdatedValue(config.CopyToTempIfOpnInAnotherProcess, "Copy to Temp Directory if File Open in Another Process");
            Front.HelpMarker("If a file is being used by another process, accessing its content is possible through a copy.\nThe application will create copies in the TEMP directory during the scan.\nThese files will be highlighted in reports (yellow font).");
            config.CheckPathKeywordPresence = ShowCheckboxAndGetUpdatedValue(config.CheckPathKeywordPresence, "Check Keyword in File Path");
            Front.HelpMarker("Checks if any of the right words exist in a file path.\nThese files will be highlighted in reports (pink font).");
            config.CheckColumnKeywordPresence = ShowCheckboxAndGetUpdatedValue(config.CheckColumnKeywordPresence, "Check Keyword in Columns");
            Front.HelpMarker("Checks if any of the right words exist in any columns of SQLite files.\nThese files will be highlighted in reports (red font).");
            ImGui.EndGroup();
            ImGui.SameLine();
            ImGui.BeginGroup();
            ImGui.SeparatorText("Keywords");
            ImGui.ListBox("", ref selectedLbID, config.ImportantKeywords.ToArray(), config.ImportantKeywords.Count(), 10);

            ImGui.InputTextWithHint("", "Case-insensitive", ref newKeyWord, 25);

            if (ImGui.Button("Add"))
            {
                if (!string.IsNullOrWhiteSpace(newKeyWord) && !config.ImportantKeywords.Contains(newKeyWord))
                {
                    config.ImportantKeywords.Add(newKeyWord.ToLower());
                }
                newKeyWord = "";
            }
            ImGui.SameLine();
            if (ImGui.Button("Remove"))
            {
                config.ImportantKeywords.Remove(config.ImportantKeywords[selectedLbID]);
            }
            ImGui.EndGroup();
        }

        private void ShowActions()
        {
            ImGui.SetCursorPosX(ImGui.GetWindowSize().X - 90);

            if (ImGui.Button("Exit"))
            {
                isOpen = false;
            }
            ImGui.SameLine();
            if (ImGui.Button("Save"))
            {
                ConfigurationManager.SaveConfiguration(config);
                isOpen = false;
            }
        }
        private bool ShowCheckboxAndGetUpdatedValue(bool currentValue, string label)
        {
            bool tmp = currentValue;
            ImGui.Checkbox(label, ref tmp);
            return tmp;
        }
    }
}
