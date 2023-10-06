using ImGuiNET;
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
        readonly FileItem sqlFileItem;
        private SQLiteReader reader;

        Table? selectedTable;
        List<DataRow> dataOfSelectedTable;

        public SQLInfoUI(FileItem sqlItem)
        {
            sqlFileItem = sqlItem;
        }

        public void Show()
        {
            if (!isOpen) return;
            ImGui.Begin("Info", ImGuiWindowFlags.NoCollapse);

            if (firstLoad)
            {
                firstLoad = !firstLoad;
                reader = new(sqlFileItem.FileInfo.FullName);
                ImGui.SetWindowSize(new Vector2(650, 600));
            }

            if (ImGui.BeginTabBar("ControlTabs", ImGuiTabBarFlags.None))
            {
                if (ImGui.BeginTabItem("File Info"))
                {
                    ImGui.Text("File Info");
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("Data"))
                {
                    ShowData();
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("Header"))
                {
                    if (sqlFileItem.FileHeader is not null)
                    {
                        Front.ShowHex(sqlFileItem.FileHeader.Header);
                    }
                    ImGui.EndTabItem();
                }
                ShowActions();
            }

            ImGui.End();
        }

        private void ShowActions()
        {
            ImGui.SetCursorPos(new Vector2(ImGui.GetWindowSize().X - 45, ImGui.GetWindowSize().Y - 30));
            if (ImGui.Button("Exit"))
            {
                isOpen = false;
            }
        }

        private void ShowData()
        {
            ImGui.BeginGroup();
            ImGui.BeginListBox("", new(150, ImGui.GetWindowSize().Y - 60));
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
                            if (cell is not null)
                            {
                                if (cell is string ||
                                    cell is int ||
                                    cell is float ||
                                    cell is double ||
                                    cell is Int64)
                                {
                                    ImGui.Text(cell.ToString());
                                }
                                else
                                {
                                    ImGui.Text(cell.ToString());
                                    Console.WriteLine("Type non géré : " + cell.GetType());
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
