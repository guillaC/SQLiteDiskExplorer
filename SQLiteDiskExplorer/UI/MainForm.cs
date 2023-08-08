using ImGuiNET;
using SQLiteDiskExplorer.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDiskExplorer.UI
{
    public class MainForm
    {

        public MainForm()
        {
            ImGui.Begin("Main Form", ImGuiWindowFlags.AlwaysAutoResize);
            ShowHeader();
            ShowDrives();
            ImGui.End();
        }

        private void ShowHeader()
        {
            ImGui.SeparatorText("Disclaimer");
            ImGui.Text("I take no responsibility for how this program is used.");
            ImGui.SeparatorText("User Guide");
            ImGui.Text($"Start by selecting the disk to analyze.\n" +
                $"Browse SQLite files from this tool or perform a global or partial export.");
        }
        private void ShowDrives()
        {
            List<DriveInfo> drives = Drive.GetDrives();
            bool[] selectedDrive = new bool[drives.Count()];


            ImGui.SeparatorText("Available Drives");
            ImGui.BeginTable("Drives", 5, ImGuiTableFlags.Resizable | ImGuiTableFlags.NoSavedSettings | ImGuiTableFlags.Borders);
            ImGui.TableSetupColumn("Name");
            ImGui.TableSetupColumn("Format");
            ImGui.TableSetupColumn("Type");
            ImGui.TableSetupColumn("Total Size");
            ImGui.TableSetupColumn("Free Space");
            ImGui.TableHeadersRow();
            int driveId = 0;

            foreach (DriveInfo drive in drives)
            {
                selectedDrive[driveId] = false;
                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                ImGui.Selectable(drive.Name, selectedDrive[driveId], ImGuiSelectableFlags.SpanAllColumns);
                ImGui.TableNextColumn();
                ImGui.Text(drive.DriveFormat);
                ImGui.TableNextColumn();
                ImGui.Text(Drive.GetDriveTypeDescription(drive.DriveType));
                ImGui.TableNextColumn();
                ImGui.Text(Drive.FormatSize(drive.TotalSize));
                ImGui.TableNextColumn();
                ImGui.Text(Drive.FormatSize(drive.TotalFreeSpace));
                driveId++;
            }

            ImGui.EndTable();
        }
    }
}
