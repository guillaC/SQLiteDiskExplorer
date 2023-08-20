using ClickableTransparentOverlay;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDiskExplorer.UI
{
    public class AboutUI
    {
        bool firstLoad = true;

        public void Show()
        {
            ImGui.Begin("About", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize);

            if (firstLoad)
            {
                firstLoad = !firstLoad;
            }


            ImGui.SeparatorText("About");
            ImGui.Text("This is a free software; it may be used and copied under the Gnu General Public License. \n");
            ImGui.Text("Github: github.com/guillaC\n" +
                "Contact: @mail");
            ImGui.Separator();
            ImGui.Text("Feel free to reach out for any inquiries, suggestions, or feedback");
            ImGui.SeparatorText("Disclaimer");
            ImGui.Text("I take no responsibility for how this program is used.");
            
            ImGui.End();
        }
    }
}
