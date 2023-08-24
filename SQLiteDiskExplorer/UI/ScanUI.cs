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

        public ScanUI(List<DriveInfo> pSelectedDrive)
        {
            foreach (var drive in pSelectedDrive)
            {
                DrivePathsMap.Add(drive, new List<FileInfo>());
                Workers.Add(drive, new SQliteScan(drive));
            }
        }

        public void Show()
        {
            ImGui.Begin("Analysis", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize);
            if (firstLoad)
            {
                firstLoad = !firstLoad;
            }

            
            ShowProgress();
            ShowAnalysis();
            ImGui.End();
        }


        public void ShowProgress()
        {
            ImGui.SeparatorText("Progress");
            foreach (var worker in Workers)
            {
                ImGui.ProgressBar(worker.Value.GetScanProgress(), new System.Numerics.Vector2(450, 20), $"{worker.Value.WorkerState}");
            }
        }

        public void LoadResult() // use timer .. 
        {
            foreach (var worker in Workers)
            {
                DrivePathsMap[worker.Key] = worker.Value.returnResult();
            }
        }

        public void ShowAnalysis()
        {
            LoadResult();
            ImGui.SeparatorText("Analysis");
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
                            ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.NoResize, 0.78f);
                            ImGui.TableSetupColumn("Date",ImGuiTableColumnFlags.NoResize, 0.15f);
                            ImGui.TableSetupColumn("Size",ImGuiTableColumnFlags.NoResize,0.07f);
                            ImGui.TableHeadersRow();
                            foreach(FileInfo file in info.Value)
                            {
                                ImGui.TableNextRow();
                                ImGui.TableNextColumn();

                                ImGui.Text(file.FullName);
                                ImGui.TableNextColumn();
                                ImGui.Text(file.CreationTime.ToString());
                                ImGui.TableNextColumn();
                                ImGui.Text(Drive.FormatSize(file.Length));
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
