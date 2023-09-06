using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDiskExplorer.Utils
{
    public static class Front
    {
        public static void HelpMarker(string tips, bool sameline = true)
        {
            if (sameline)
            {
                ImGui.SameLine();
            }
            ImGui.TextDisabled("(?)");
            if (ImGui.IsItemHovered())
            {
                ImGui.BeginTooltip();
                ImGui.Text(tips); 
                ImGui.EndTooltip();
            }
        }
    }
}
