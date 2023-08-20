using ClickableTransparentOverlay;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDiskExplorer.UI
{
    public class ScanUI
    {
        bool firstLoad = true;
        Dictionary<DriveInfo, List<string>> DrivePathsMap = new();

        public ScanUI(List<DriveInfo> pSelectedDrive)
        {
            foreach (var drive in pSelectedDrive)
            {
                DrivePathsMap.Add(drive, new List<string>());
            }
        }

        public void Show()
        {
            ImGui.Begin("Analysis", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize);

            if (firstLoad)
            {
                firstLoad = !firstLoad;
            }

            ImGui.SeparatorText("Analysis");
            ShowAnalysis();
            ImGui.End();
        }


        public void ShowAnalysis()
        {
            if (ImGui.BeginTabBar("ControlTabs", ImGuiTabBarFlags.None))
            {

                foreach (KeyValuePair<DriveInfo, List<String>> info in DrivePathsMap)
                {
                    var drive = info.Key;

                    if (ImGui.BeginTabItem(drive.Name))
                    {
                        ImGui.Text(drive.Name);
                        ImGui.EndTabItem();
                    }
                }


            }
        }
    }
}
