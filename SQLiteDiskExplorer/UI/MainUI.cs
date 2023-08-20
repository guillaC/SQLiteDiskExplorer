using ClickableTransparentOverlay;
using ImGuiNET;
using SixLabors.ImageSharp.PixelFormats;
using SQLiteDiskExplorer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SQLiteDiskExplorer.UI
{
    public class MainUI
    {
        const string IMGP = ".\\Res\\SQLiteDiskExplorerIcon.png";

        RenderControllerClass RenderControllerClass;

        bool firstLoad = true;

        bool[]? selectedDrive;
        List<DriveInfo> drives = new();

        bool srbg = true;
        IntPtr imageHandle;
        uint imageHeight, imageWidth;

        public MainUI(RenderControllerClass rcClass)
        {
            RenderControllerClass = rcClass;
        }

        public void Show()
        {
            ImGui.Begin("Main Form", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.MenuBar);

            if (firstLoad)
            {
                ImGui.StyleColorsClassic();
                RenderControllerClass.AddOrGetImagePointer(IMGP, srbg, out imageHandle, out imageWidth, out imageHeight);
                InitializeSelectedDriveList();
                firstLoad = !firstLoad;
            }

            ShowMenuBar();
            ShowHeader();
            ShowDrives();
            ShowActions();

            ImGui.End();
        }

        private void ShowMenuBar()
        {
            ImGui.BeginMenuBar();

            ImGui.PushStyleColor(ImGuiCol.Button, new System.Numerics.Vector4());

            if (ImGui.MenuItem("About"))
            {
                ShowAboutForm();
            }

            ImGui.Separator();
            if (ImGui.MenuItem("Exit", "Alt+F4"))
            {
                Environment.Exit(0);
            }

            ImGui.PopStyleColor();
            ImGui.EndMenuBar();
        }

        private void ShowHeader()
        {

            ImGui.BeginChild("ChildImg", new System.Numerics.Vector2(100, 100));
            ImGui.Image(imageHandle, new System.Numerics.Vector2(100, 100));
            ImGui.EndChild();
            ImGui.SameLine();
            ImGui.BeginChild("ChildGuide", new System.Numerics.Vector2(620, 100));
            ImGui.SeparatorText("User Guide");
            ImGui.Text("To begin, select one or more disks for analysis.\n" +
                   "You can browse SQLite files within this tool or perform a global or partial export.\n" +
                   "The exported files will be saved in a directory named based on the current date,\n" +
                   "which is located in the same directory as the application.\n" +
                   "For complete results, run the application as an admin when scanning the system disk.");
            ImGui.EndChild();
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
                List<DriveInfo> selectedDrives = drives
    .Select((drive, index) => new { Drive = drive, Index = index })
    .Where(item => selectedDrive[item.Index])
    .Select(item => item.Drive)
    .ToList();
                RenderControllerClass.scanForm = new ScanUI(selectedDrives);
            }
        }
        private void ShowAboutForm()
        {
            RenderControllerClass.aboutForm = new AboutUI();
        }

        private void InitializeSelectedDriveList()
        {
            drives = Drive.GetDrives();
            selectedDrive = new bool[drives.Count()];
        }
    }
}
