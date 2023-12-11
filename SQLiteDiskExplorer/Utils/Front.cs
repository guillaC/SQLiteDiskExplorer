using ImGuiNET;
using SQLiteDiskExplorer.UI;
using System.Numerics;
using System.Text;


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

        public static void ShowStringFromHex(byte[] data)
        {
            List<string> stringsList = StringHex.ExtractStrings(data);

            ImGui.BeginTable("Strings", 2, ImGuiTableFlags.Resizable | ImGuiTableFlags.NoSavedSettings | ImGuiTableFlags.Borders);
            ImGui.TableSetupColumn("index", ImGuiTableColumnFlags.None, 0.07f);
            ImGui.TableSetupColumn("string", ImGuiTableColumnFlags.None, 0.93f);

            ImGui.TableHeadersRow();

            int index = 0;

            foreach (string s in stringsList)
            {
                index++;
                ImGui.TableNextRow();
                ImGui.TableNextColumn();
                ImGui.Text(index.ToString());
                ImGui.TableNextColumn();
                ImGui.Text(s);
            }

            ImGui.EndTable();
        }

        public static void ShowHexToString(byte[] data)
        {
            ImGui.BeginTable("HexToString", 16, ImGuiTableFlags.Resizable | ImGuiTableFlags.NoSavedSettings | ImGuiTableFlags.Borders, new Vector2(260, 0));

            for (int i = 0; i <= 0xF; i++)
            {
                ImGui.TableSetupColumn(i.ToString("X"), ImGuiTableColumnFlags.NoResize);
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
                        if (data[index] == byte.MinValue)
                        {
                            ImGui.TextColored((Vector4)Color.Gray, ".");
                        }
                        else
                        {
                            ImGui.TextColored((Vector4)Color.WhiteSmoke, Convert.ToChar(data[index]).ToString());
                        }
                    }
                    else
                    {
                        ImGui.Text("");
                    }
                }
            }

            ImGui.EndTable();
        }

        public static void ShowHex(byte[] data)
        {
            ImGui.BeginTable("Hex", 16, ImGuiTableFlags.Resizable | ImGuiTableFlags.NoSavedSettings | ImGuiTableFlags.Borders, new Vector2(500, 0));

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
