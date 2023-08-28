using ImGuiNET;
using SQLiteDiskExplorer.Core;
using SQLiteDiskExplorer.Utils;
using System.Numerics;

namespace SQLiteDiskExplorer.UI
{
    public class ScanUI
    {
        bool firstLoad = true; 
        bool isOpen = true;
        bool cancelProcessing = false;

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
            if (!isOpen) return;
            ImGui.Begin("Analysis", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize);
            if (firstLoad)
            {
                ApplyStyle();
                firstLoad = !firstLoad;
            }

            ShowProgress();
            ShowScannerActions();

            ShowAnalysis();

            ImGui.End();
        }

        private void ApplyStyle()
        {
            // ImGui.PushStyleColor(ImGuiCol.PlotHistogram, new Vector4(11, 22, 33, 44));
        }

        private void ShowProgress()
        {
            ImGui.SeparatorText("Progress");
            foreach (var worker in Workers)
            {
                ImGui.ProgressBar(worker.Value.GetScanProgress(), new System.Numerics.Vector2(450, 20), $"{worker.Value.WorkerState}");
            }
        }

        private void ShowScannerActions()
        {
            ImGui.SeparatorText("Actions");

            if (!cancelProcessing)
            {
                if (ImGui.Button("Stop"))
                {
                    CancelWorker();
                }
            }

            ImGui.SameLine();

            if (ImGui.Button("Exit"))
            {
                if (!cancelProcessing) CancelWorker();
                isOpen = false;
            }
        }

        private void LoadResult()
        {
            foreach (var worker in Workers)
            {
                DrivePathsMap[worker.Key] = worker.Value.returnResult();
            }
        }

        private void CancelWorker()
        {
            cancelProcessing = true;
            foreach (var worker in Workers)
            {
                worker.Value.StopScan();
            }
        }

        private void ShowAnalysis()
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
                                ImGui.TableNextColumn();
                                ImGui.Text(file.FullName);
                                ImGui.TableNextColumn();
                                ImGui.Text(file.CreationTime.ToString());
                                ImGui.TableNextColumn();
                                ImGui.Text(Drive.FormatSize(file.Length));
                                ImGui.TableNextRow();
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
