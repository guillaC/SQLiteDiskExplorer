using ClickableTransparentOverlay;
using ImGuiNET;
using SQLiteDiskExplorer.Core;
using SQLiteDiskExplorer.Utils;
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
        Dictionary<DriveInfo, List<FileInfo>> DrivePathsMap = new();
        Dictionary<DriveInfo, SQliteScan> Workers = new();

        float progress = 0.0f;
        string progressStr = "c'est en cours le zin";

        public ScanUI(List<DriveInfo> pSelectedDrive)
        {
            foreach (var drive in pSelectedDrive)
            {
                DrivePathsMap.Add(drive, new List<FileInfo>());
                Workers.Add(drive, new SQliteScan(drive));

                // start scan
            }
        }

        public void Show()
        {
            ImGui.Begin("Analysis", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize);


            if (firstLoad)
            {
                firstLoad = !firstLoad;
            }

            LoadResult();

            ImGui.SeparatorText("Progress");
            ShowProgress();
            ImGui.SeparatorText("Analysis");
            ShowAnalysis();
            ImGui.End();

            
        }


        public void ShowProgress()
        {
            ImGui.ProgressBar(progress, new System.Numerics.Vector2(450,20), progressStr);
        }

        public void LoadResult()
        {
            foreach (var worker in Workers)
            {
                DrivePathsMap[worker.Key] = worker.Value.returnResult();
            }
        }

        public void ShowAnalysis()
        {
            if (ImGui.BeginTabBar("ControlTabs", ImGuiTabBarFlags.None))
            {
                foreach (KeyValuePair<DriveInfo, List<FileInfo>> info in DrivePathsMap)
                {
                    var drive = info.Key;

                    if (ImGui.BeginTabItem(drive.Name))
                    {
                        ImGui.SeparatorText("Result");
                        if (ImGui.BeginChild($"Result {drive.Name}", new System.Numerics.Vector2(1200, 650)))
                        {
                            ImGui.BeginTable("Files", 3, ImGuiTableFlags.Resizable | ImGuiTableFlags.NoSavedSettings | ImGuiTableFlags.Borders);
                            ImGui.TableSetupColumn("Name");
                            ImGui.TableSetupColumn("Date");
                            ImGui.TableSetupColumn("Size");
                            ImGui.TableHeadersRow();

                            foreach(FileInfo file in info.Value)
                            {
                                ImGui.TableNextRow();
                                ImGui.TableNextColumn();

                                ImGui.Text(file.FullName);
                                ImGui.TableNextColumn();
                                ImGui.Text(file.CreationTime.ToString());
                                ImGui.TableNextColumn();
                                ImGui.Text(file.Length.ToString());
                                ImGui.TableNextColumn();
                            }

                            ImGui.EndTable();

                            ImGui.EndChild();
                        }
                        ImGui.EndTabItem();
                    }
                }


            }
        }
    }
}
