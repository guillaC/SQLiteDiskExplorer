using ClickableTransparentOverlay;
using ImGuiNET;
using SQLiteDiskExplorer.Core;
using SQLiteDiskExplorer.Model;
using SQLiteDiskExplorer.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDiskExplorer.UI
{
    public class SQLInfoUI
    {
        bool firstLoad = true;
        bool isOpen = true;
        FileItem sqlFileItem;

        public SQLInfoUI(FileItem sqlItem)
        {
            sqlFileItem = sqlItem;
        }

        public void Show()
        {
            if (!isOpen) return;
            ImGui.Begin("LELELEL", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize);

            if (firstLoad)
            {
                firstLoad = !firstLoad;
            }

            ImGui.SeparatorText("Header");
            ImGui.Text($"SchemaFormatNumber : {sqlFileItem.FileHeader.SchemaFormatNumber}");
            ImGui.Text($"UserVersion : {sqlFileItem.FileHeader.UserVersion}");
            ImGui.Text($"SQLiteVersionNumber : {sqlFileItem.FileHeader.SQLiteVersionNumber}");

            ImGui.SameLine();

            ImGui.SetCursorPosX(ImGui.GetWindowSize().X - 45);
            if (ImGui.Button("Exit"))
            {
                isOpen = false;
            }

            ImGui.End();
        }

        public void ShowActions()
        {
            if (ImGui.Button("Exit"))
            {
                isOpen = false;
            }
        }
    }
}
