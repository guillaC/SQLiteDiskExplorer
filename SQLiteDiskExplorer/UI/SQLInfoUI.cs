using ClickableTransparentOverlay;
using ImGuiNET;
using SQLiteDiskExplorer.Core;
using SQLiteDiskExplorer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDiskExplorer.UI
{
    public class SQLInfoUI
    {
        bool firstLoad = true;
        bool isOpen = true;
        FileItem FileItem;

        public SQLInfoUI(FileItem fileitem)
        {
            FileItem = fileitem;
        }

        public void Show()
        {
            if (!isOpen) return;
            ImGui.Begin("Information", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize);

            if (firstLoad)
            {
                firstLoad = !firstLoad;
            }

            ShowActions();
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
