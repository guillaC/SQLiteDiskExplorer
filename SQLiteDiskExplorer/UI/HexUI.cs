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
            float buttonWidth = ImGui.CalcTextSize("Exit").X + 2.0f * ImGui.GetStyle().FramePadding.X;
            ImGui.SetCursorPosX(ImGui.GetWindowSize().X - buttonWidth - ImGui.GetStyle().ItemSpacing.X);
            if (ImGui.Button("Exit"))
            {
                isOpen = false;
            }
            ImGui.SameLine();
        }
    }
}
