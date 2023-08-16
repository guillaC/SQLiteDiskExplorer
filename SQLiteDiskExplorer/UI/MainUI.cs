using ImGuiNET;
using ClickableTransparentOverlay;
using SQLiteDiskExplorer.Utils;

namespace SQLiteDiskExplorer.UI
{
    public class MainUI : Overlay
    {
        bool firstLoad = true;

        List<DriveInfo> drives = new();
        bool[]? selectedDrive;

        protected override void Render()
        {
            ImGui.Begin("Main Form", ImGuiWindowFlags.NoCollapse);

            if (firstLoad)
            {
                ImGui.StyleColorsDark();
                ImGui.SetWindowSize(new System.Numerics.Vector2(700, 500));
                InitializeSelectedDriveList();
                firstLoad = !firstLoad;
            }
            
            ShowHeader();
            ShowDrives();
            ShowActions();
            ImGui.End();
        }

        private void ShowHeader()
        {
            ImGui.SeparatorText("Disclaimer");
            ImGui.Text("I take no responsibility for how this program is used.");
            ImGui.SeparatorText("User Guide");
            ImGui.Text("To begin, select one or more disks for analysis.\n" +
                   "You can browse SQLite files within this tool or perform a global or partial export.\n" +
                   "The exported files will be saved in a directory named based on the current date,\n" +
                   "which is located in the same directory as the application.\n" +
                   "For complete results, run the application as an admin when scanning the system disk.");
        }
        private void ShowDrives()
        {
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
                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                ImGui.Selectable(drive.Name, selectedDrive[driveId], ImGuiSelectableFlags.SpanAllColumns);

                if (ImGui.IsItemClicked())
                {
                    selectedDrive[driveId] = !selectedDrive[driveId];
                }

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

        private void ShowActions()
        {
            if (ImGui.Button("Process"))
            {

            }
            ImGui.SameLine();
            if (ImGui.Button("Properties"))
            {

            }
            ImGui.SameLine();
            if (ImGui.Button("Exit"))
            {
                Console.WriteLine("Exit");
            }
        }

        private void InitializeSelectedDriveList()
        {
            drives = Drive.GetDrives();
            selectedDrive = new bool[drives.Count()];
        }
    }
}
