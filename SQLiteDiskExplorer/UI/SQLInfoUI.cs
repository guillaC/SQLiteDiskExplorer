using ImGuiNET;
using SixLabors.ImageSharp;
using SQLiteDiskExplorer.Core;
using SQLiteDiskExplorer.Model;
using SQLiteDiskExplorer.Model.Schema;
using SQLiteDiskExplorer.Utils;
using System.Data;
using System.Numerics;

namespace SQLiteDiskExplorer.UI
{
    public class SQLInfoUI
    {
        bool firstLoad = true;
        bool isOpen = true;
        byte[]? fileHex;
        readonly FileItem sqlFileItem;
        private SQLiteReader reader;
        Table? selectedTable;
        List<DataRow> dataOfSelectedTable = new();
        bool popupIsOpen;

        public SQLInfoUI(FileItem sqlItem)
        {
            try
            {
                fileHex = ReadSave.ReadFile(filePath: sqlItem.FileInfo!.FullName!);
                sqlFileItem = sqlItem;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Show()
        {
            if (!isOpen) return;
            ImGui.Begin("Info", ImGuiWindowFlags.NoCollapse);

            if (firstLoad)
            {
                firstLoad = !firstLoad;
                reader = new(sqlFileItem.FileInfo.FullName);
                reader.LoadTableStructure();

                ImGui.SetWindowSize(new Vector2(650, 610));
            }

            if (ImGui.BeginTabBar("ControlTabs", ImGuiTabBarFlags.None))
            {
                if (ImGui.BeginTabItem("Data"))
                {
                    ShowData();
                    ShowActions();
                    ImGui.EndTabItem();
                }

                if (fileHex?.Length > 0)
                {
                    if (ImGui.BeginTabItem("Hex"))
                    {
                        ImGui.BeginGroup();
                        Front.ShowHex(fileHex);
                        ImGui.EndGroup();
                        ImGui.SameLine();
                        ImGui.BeginGroup();
                        Front.ShowHexToString(fileHex);
                        ImGui.EndGroup();
                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem("Strings"))
                    {
                        ImGui.BeginGroup();
                        Front.ShowStringFromHex(fileHex);
                        ImGui.EndGroup();
                        ImGui.EndTabItem();
                    }
                }
            }

            ImGui.EndTabBar();
            ImGui.End();
        }

        private void ShowActions()
        {
            if (ImGui.Button("Save"))
            {
                string targetPath = Path.Combine($"{Environment.CurrentDirectory}\\Export", $"{Path.GetFileNameWithoutExtension(sqlFileItem.FileInfo.Name)}_{DateTime.Now:yyyyMMdd_HHmmss}_{Guid.NewGuid().ToString().Substring(1, 5)}.sqlite");
                ReadSave.CopyFile(sqlFileItem.FileInfo.FullName, targetPath);

                popupIsOpen = true;
                ImGui.OpenPopup("saved");
            }

            ImGui.SameLine();
            if (ImGui.Button("Exit"))
            {
                isOpen = false;
            }

            if (ImGui.BeginPopupModal("saved", ref popupIsOpen, ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoSavedSettings))
            {
                ImGui.Text($"The SQLite file has been saved in the directory:");
                ImGui.Text($"{Environment.CurrentDirectory}\\Export");

                float buttonWidth = ImGui.CalcTextSize("Ok").X + 2.0f * ImGui.GetStyle().FramePadding.X;
                ImGui.SetCursorPosX(ImGui.GetWindowSize().X - buttonWidth - ImGui.GetStyle().ItemSpacing.X);

                if (ImGui.Button("Ok"))
                {
                    ImGui.CloseCurrentPopup();
                }
                ImGui.EndPopup();
            }
        }

        private void ShowData()
        {
            ImGui.BeginGroup();
            ImGui.BeginListBox("", new(150, ImGui.GetWindowSize().Y - 85));
            foreach (Table table in reader.Schema)
            {
                if (ImGui.Selectable(table.TableName))
                {
                    Console.WriteLine(table.TableName);
                    UpdateTableData(table);
                }
            }
            ImGui.EndListBox();
            ImGui.EndGroup();
            ImGui.SameLine();
            ImGui.BeginGroup();
            ImGui.SeparatorText("Data");

            if (selectedTable is not null) UpdateTableData(selectedTable);
            ShowTableData();
            ImGui.EndGroup();
        }

        private void UpdateTableData(Table table)
        {
            selectedTable = null;
            dataOfSelectedTable = reader.GetTableData(table);
        }

        private void ShowTableData()
        {
            int i = 0;
            if (dataOfSelectedTable is not null && dataOfSelectedTable.Count > 0)
            {
                ImGui.BeginChild("tableData", new(ImGui.GetWindowSize().X - 170, ImGui.GetWindowSize().Y - 110));
                ImGui.Columns(dataOfSelectedTable[0].ItemArray.Length);
                foreach (DataColumn column in dataOfSelectedTable[0].Table.Columns)
                {
                    ImGui.Text(column.ColumnName);
                    ImGui.NextColumn();
                }

                ImGui.Separator();

                foreach (DataRow row in dataOfSelectedTable)
                {
                    foreach (var cell in row.ItemArray)
                    {
                        try
                        {
                            if (cell is not DBNull && cell is not null)
                            {
                                switch (cell)
                                {
                                    case int intValue:
                                    case string stringValue:
                                    case Int64 int64Value:
                                    case float floatValue:
                                    case double doubleValue:
                                    case decimal decimalValue:
                                        ImGui.TextUnformatted(cell.ToString());
                                        break;
                                    case bool boolValue:
                                        var color = boolValue ? Color.GreenYellow : Color.Red;
                                        ImGui.TextColored((Vector4)color, cell.ToString());
                                        break;
                                    case byte[] byteValue:

                                        ImGui.PushID($"Hex#{i++}");
                                        if (ImGui.Button($"byte[{byteValue.Length}]"))
                                        {
                                            RenderControllerClass.hexUIForm = new HexUI((byte[]?)cell);
                                        }
                                        ImGui.PopID();
                                        break;
                                    default:
                                        Console.WriteLine("Unhandled type: " + cell.GetType() + ": " + cell.ToString());
                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            ImGui.Text("BUG");
                        }
                        ImGui.NextColumn();
                    }
                }
                ImGui.EndChild();
            }
        }
    }
}
