﻿using ImGuiNET;
using SQLiteDiskExplorer.Model;
using SQLiteDiskExplorer.Utils;
using System.Numerics;
using Vulkan;

namespace SQLiteDiskExplorer.UI
{
    public class SQLInfoUI
    {
        bool firstLoad = true;
        bool isOpen = true;
        FileItem sqlFileItem;

        public SQLInfoUI(FileItem sqlItem)
        {
            sqlFileItem = sqlItem;
        }

        public void Show()
        {
            if (!isOpen) return;
            ImGui.Begin("Info", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoResize);

            if (firstLoad)
            {
                firstLoad = !firstLoad;
                ImGui.SetWindowSize(new Vector2(350, 400));
            }

            if (ImGui.BeginTabBar("ControlTabs", ImGuiTabBarFlags.None))
            {
                if (ImGui.BeginTabItem("File Info"))
                {
                    ImGui.Text("Woow");
                    ImGui.EndTabItem();
                }

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
