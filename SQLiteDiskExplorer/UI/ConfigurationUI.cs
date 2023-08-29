using ClickableTransparentOverlay;
using ImGuiNET;
using SQLiteDiskExplorer.Model;
using SQLiteDiskExplorer.Utils;
using AppConfig = SQLiteDiskExplorer.Model.AppConfig;

namespace SQLiteDiskExplorer.UI
{
    public class ConfigurationUI
    {
        bool firstLoad = true;
        bool isOpen = true;
        int selectedLbID;

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
            ImGui.SeparatorText("Configuration");
            config.RecurseSubdirectories = ShowCheckboxAndGetUpdatedValue(config.RecurseSubdirectories, "Scan Subdirectories");
            config.IgnoreInaccessible = ShowCheckboxAndGetUpdatedValue(config.IgnoreInaccessible, "Ignore Inaccessible Files");
            config.CopyToTempIfOpnInAnotherProcess = ShowCheckboxAndGetUpdatedValue(config.CopyToTempIfOpnInAnotherProcess, "Copy to Temp Directory if File Open in Another Process");
            config.CheckPathKeywordPresence = ShowCheckboxAndGetUpdatedValue(config.CheckPathKeywordPresence, "Check Keyword in File Path");
            config.CheckColumnKeywordPresence = ShowCheckboxAndGetUpdatedValue(config.CheckColumnKeywordPresence, "Check Keyword in Columns");
            ImGui.EndGroup();

            ImGui.SameLine();

            ImGui.BeginGroup();
            ImGui.SeparatorText("Keywords");
            ImGui.ListBox("", ref selectedLbID, config.ImportantKeywords.ToArray(), config.ImportantKeywords.Count(), 10);


            if (ImGui.Button("+"))
            {
                
            }
            ImGui.SameLine();
            if (ImGui.Button("-"))
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
