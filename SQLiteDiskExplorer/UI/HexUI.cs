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
                ImGui.SetWindowSize(new Vector2(1500, 700));
                firstLoad = !firstLoad;
            }
            
            
            /*
             * TODO : debug ici
             */

            Front.ShowHex(data);
            ImGui.End();
        }

    }
}
