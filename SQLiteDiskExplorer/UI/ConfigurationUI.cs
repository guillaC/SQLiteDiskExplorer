using ClickableTransparentOverlay;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDiskExplorer.UI
{
    public class ConfigurationUI
    {
        bool firstLoad = true;
        bool isOpen = true;

        public void Show()
        {
            if (!isOpen) return;
            ImGui.Begin("Configuration", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize);

            if (firstLoad)
            {
                firstLoad = !firstLoad;
            }

            ShowCurrentConfiguration();
            ShowActions();
            ImGui.End();
        }

        public void ShowCurrentConfiguration()
        {
            ImGui.SeparatorText("Configuration");

            // Load Config & show

            ImGui.Text("test");

        }
        public void ShowActions()
        {
            if (ImGui.Button("Exit"))
            {
                isOpen = false;
            }
            ImGui.SameLine();
            if (ImGui.Button("Save"))
            {

            }
        }
    }
}
