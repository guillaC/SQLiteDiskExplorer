using ImGuiNET;
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
    public class HexUI
    {
        bool firstLoad = true;
        bool isOpen = true;
        byte[] data;

        public HexUI(byte[]? pData)
        {
            data = pData ?? Array.Empty<byte>();
        }

        public void Show()
        {
            if (!isOpen) return;
            ImGui.Begin("Hex Viewer", ImGuiWindowFlags.NoCollapse);

            if (firstLoad)
            {
                firstLoad = !firstLoad;
            }

            ImGui.BeginGroup();
            Front.ShowHex(data);
            ImGui.EndGroup();

            ImGui.SameLine();

            ImGui.BeginGroup();
            Front.ShowHexToString(data);
            ImGui.EndGroup();


            ShowActions();
            ImGui.End();
        }


        private void ShowActions()
        {
            ImGui.SetCursorPosX(ImGui.GetWindowSize().X - 90);

            if (ImGui.Button("Exit"))
            {
                isOpen = false;
            }
            ImGui.SameLine();
            if (ImGui.Button("Save"))
            {
                //todo
                isOpen = false;
            }
        }

    }
}
