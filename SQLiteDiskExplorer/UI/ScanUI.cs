using ImGuiNET;
using SQLiteDiskExplorer.Core;
using SQLiteDiskExplorer.Model;
using SQLiteDiskExplorer.Utils;
using System.Data.Common;
using System.Numerics;

namespace SQLiteDiskExplorer.UI
{
    public class ScanUI
    {
        bool firstLoad = true;
        bool isOpen = true;
        bool cancelProcessing = false;

        Dictionary<DriveInfo, List<FileItem>> DrivePathsMap = new();
        Dictionary<DriveInfo, SQliteScan> Workers = new();

        AppConfig config;

        public ScanUI(List<DriveInfo> pSelectedDrive)
        {
            config = ConfigurationManager.LoadConfiguration()!;

            foreach (string ze in config.ImportantKeywords)
            {
                Console.WriteLine(ze);
            }

            foreach (var drive in pSelectedDrive)
            {
                DrivePathsMap.Add(drive, new List<FileItem>());
                Workers.Add(drive, new SQliteScan(drive));
            }
        }

        public void Show()
        {
            if (!isOpen) return;
            ImGui.Begin("Analysis", ImGuiWindowFlags.NoCollapse);
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
            ImGui.SetWindowSize(new Vector2(1500, 700));
            ImGui.PushStyleColor(ImGuiCol.PlotHistogram, (Vector4)Color.CadetBlue);
        }

        private void ShowProgress()
        {
            ImGui.SeparatorText("Progress");
            foreach (var worker in Workers)
            {
                ImGui.ProgressBar(worker.Value.GetScanProgress(), new Vector2(450, 20), $"{worker.Value.WorkerState}");
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
                ImGui.SameLine();
            }

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
                DrivePathsMap[worker.Key] = worker.Value.ReturnResult();
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
                foreach (KeyValuePair<DriveInfo, List<FileItem>> info in DrivePathsMap)
                {
                    var drive = info.Key;

                    if (ImGui.BeginTabItem(drive.Name))
                    {
                        ImGui.SeparatorText("Result");
                        if (ImGui.BeginChild($"Result {drive.Name}"))
                        {
                            ImGui.BeginTable("Files", 4, ImGuiTableFlags.Resizable | ImGuiTableFlags.NoSavedSettings | ImGuiTableFlags.Borders);
                            ImGui.TableSetupColumn("Name", ImGuiTableColumnFlags.None, 0.65f);
                            ImGui.TableSetupColumn("Date", ImGuiTableColumnFlags.NoResize, 0.15f);
                            ImGui.TableSetupColumn("Size", ImGuiTableColumnFlags.NoResize, 0.07f);
                            ImGui.TableSetupColumn("Action", ImGuiTableColumnFlags.NoResize, 0.13f);
                            ImGui.TableHeadersRow();
                            foreach (FileItem file in info.Value)
                            {
                                ImGui.TableNextColumn();
                                if (config.CheckPathKeywordPresence && config.ImportantKeywords.Any(keyword => file.FileInfo.FullName.Contains(keyword.ToLower())))
                                {
                                    ImGui.TextColored((Vector4)Color.BlueViolet, "[KeywordInPath]");
                                    ImGui.SameLine();
                                }

                                if (config.CheckColumnKeywordPresence && (file.ColumnKeywordPresence is not null && file.ColumnKeywordPresence.Any()))
                                {
                                    foreach (KeyValuePair<string, List<string>> found in file.ColumnKeywordPresence)
                                    {
                                        ImGui.TextColored((Vector4)Color.RoyalBlue, $"[KeywordInColumn({found.Key})]");
                                        if (ImGui.IsItemHovered())
                                        {
                                            ImGui.SetTooltip($" {String.Join(",", found.Value)}");
                                        }
                                        ImGui.SameLine();
                                    }
                                }

                                ImGui.Text(file.FileInfo.FullName);
                                ImGui.TableNextColumn();
                                ImGui.Text(file.FileInfo.CreationTime.ToString());
                                ImGui.TableNextColumn();
                                ImGui.Text(Drive.FormatSize(file.FileInfo.Length));
                                ImGui.TableNextColumn();

                                ImGui.PushID($"Information#{file.FileInfo.FullName}");

                                if (ImGui.SmallButton("Information"))
                                {
                                    Console.WriteLine("Information");
                                    RenderControllerClass.infoForm = new SQLInfoUI(file);
                                }
                                ImGui.SameLine();
                                if (ImGui.SmallButton("Open"))
                                {
                                    Console.WriteLine("Open");
                                }

                                ImGui.PopID();

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
