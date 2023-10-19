using ImGuiNET;
using SQLiteDiskExplorer.Utils;

namespace SQLiteDiskExplorer.UI
{
    public class HexUI
    {
        bool firstLoad = true;
        bool isOpen = true;
        private readonly byte[] data;

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
            ImGui.SetCursorPosX(ImGui.GetWindowSize().X - 50);

            if (ImGui.Button("Exit"))
            {
                isOpen = false;
            }
            ImGui.SameLine();
        }
    }
}
