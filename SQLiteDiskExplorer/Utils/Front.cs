using ImGuiNET;
using System.Numerics;

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

        //TODO
        public static void ShowHexToString(byte[] data)
        {

        }

        public static void ShowHex(byte[] data)
        {
            ImGui.BeginTable("Hex", 16, ImGuiTableFlags.Resizable | ImGuiTableFlags.NoSavedSettings | ImGuiTableFlags.Borders);

            Console.WriteLine($"data length : {data.Length}");
            Console.WriteLine($"tostring : {data}");

            for (int i = 0; i <= 0xF; i++)
            {
                ImGui.TableSetupColumn(" " + i.ToString("X"), ImGuiTableColumnFlags.NoResize);
            }

            ImGui.TableHeadersRow();

            int valuesPerRow = 16;

            for (int i = 0; i < data.Length; i += valuesPerRow)
            {
                ImGui.TableNextRow();
                for (int j = 0; j < valuesPerRow; j++)
                {
                    ImGui.TableNextColumn();
                    int index = i + j;
                    if (index < data.Length)
                    {
                        var oct = data[index].ToString("X2");
                        ImGui.TextColored(oct == "00" ? (Vector4)Color.Gray : (Vector4)Color.WhiteSmoke, oct);
                    }
                    else
                    {
                        ImGui.Text("");
                    }
                }
            }

            ImGui.EndTable();
        }
    }
}
