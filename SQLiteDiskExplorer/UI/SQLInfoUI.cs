using ImGuiNET;
using SQLiteDiskExplorer.Core;
using SQLiteDiskExplorer.Model;
using SQLiteDiskExplorer.Model.Schema;
using SQLiteDiskExplorer.Utils;
using System.Numerics;

namespace SQLiteDiskExplorer.UI
{
    public class SQLInfoUI
    {
        bool firstLoad = true;
        bool isOpen = true;
        readonly FileItem sqlFileItem;
        private SQLiteReader reader;

        int selectedTable=0;

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
                    ImGui.BeginGroup();
                    ImGui.BeginListBox("",new(150, ImGui.GetWindowSize().Y-90));
                    foreach (Table table in reader.Schema)
                    {
                        if (ImGui.Selectable(table.TableName))
                        {
                            Console.WriteLine(table.TableName);
                        }
                    }
                    ImGui.EndListBox();
                    ImGui.EndGroup();
                    ImGui.SameLine();
                    ImGui.BeginGroup();
                    ImGui.SeparatorText("Data");
                    ImGui.Text("Data");
                    ImGui.EndGroup();
                    ImGui.EndTabItem();
                }

                if (ImGui.BeginTabItem("Query"))
                {
                    ImGui.Text("Query");
                    ImGui.EndTabItem();
                }

                /*
                if (ImGui.BeginTabItem("Structure"))
                {
                    imnodes.BeginNodeEditor();
                    imnodes.BeginNode(1);
                    imnodes.BeginInputAttribute(2);
                    ImGui.Text("input");
                    imnodes.EndInputAttribute();
                    imnodes.BeginOutputAttribute(3);
                    ImGui.Indent(40);  // Push the text label to the right side.
                                        // At the moment UI elements dont't get
                                        // aligned automatically within the nodes.
                    ImGui.Text("output");
                    imnodes.EndOutputAttribute();
                    imnodes.EndNode();
                    imnodes.EndNodeEditor();
                    ImGui.EndTabItem();
                }
                */

                if (ImGui.BeginTabItem("Header"))
                {
                    if (sqlFileItem.FileHeader is not null)
                    {
                        Front.ShowHex(sqlFileItem.FileHeader.Header);
                    }
                    else
                    {
                        ImGui.Text("Something went wrong");
                    }

                    /*
                    ImGui.Text($"Header=header                                 : {sqlFileItem.FileHeader.Header}");
                    ImGui.Text($"PageSize=ToUInt16(header,16)                  : {sqlFileItem.FileHeader.PageSize}");
                    ImGui.Text($"FileFormatWriteVersion=header[18]             : {sqlFileItem.FileHeader.FileFormatWriteVersion}");
                    ImGui.Text($"FileFormatReadVersion=header[19]              : {sqlFileItem.FileHeader.FileFormatReadVersion}");
                    ImGui.Text($"FileChangeCounter=ToUInt32(header,24)         : {sqlFileItem.FileHeader.FileChangeCounter}");
                    ImGui.Text($"DatabaseSizeInPages=ToUInt32(header,28)       : {sqlFileItem.FileHeader.DatabaseSizeInPages}");
                    ImGui.Text($"FirstFreelistTrunkPage=ToUInt32(header,32)    : {sqlFileItem.FileHeader.FirstFreelistTrunkPage}");
                    ImGui.Text($"TotalFreelistPages=ToUInt32(header,36)        : {sqlFileItem.FileHeader.TotalFreelistPages}");
                    ImGui.Text($"SchemaCookie=ToUInt32(header,40)              : {sqlFileItem.FileHeader.SchemaCookie}");
                    ImGui.Text($"SchemaFormatNumber=ToUInt32(header,44)        : {sqlFileItem.FileHeader.SchemaFormatNumber}");
                    ImGui.Text($"DefaultPageCacheSize=ToUInt32(header,48)      : {sqlFileItem.FileHeader.DefaultPageCacheSize}");
                    ImGui.Text($"UserVersion=ToUInt32(header,60)               : {sqlFileItem.FileHeader.UserVersion}");
                    ImGui.Text($"VersionValidForNumber=ToUInt32(header,92)     : {sqlFileItem.FileHeader.VersionValidForNumber}");
                    ImGui.Text($"SQLiteVersionNumber=ToUInt32(header,96)       : {sqlFileItem.FileHeader.SQLiteVersionNumber}");
                    */

                    ImGui.EndTabItem();
                }
            }

            ImGui.SetCursorPosX(ImGui.GetWindowSize().X - 45);
            if (ImGui.Button("Exit"))
            {
                isOpen = false;
            }

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
