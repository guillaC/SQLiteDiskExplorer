using ImGuiNET;

namespace SQLiteDiskExplorer.UI
{
    public class AboutUI
    {
        bool firstLoad = true;
        bool isOpen = true;

        public void Show()
        {
            if (!isOpen) return;
            ImGui.Begin("About", ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.AlwaysAutoResize);

            if (firstLoad)
            {
                firstLoad = !firstLoad;
            }

            ImGui.SeparatorText("About");
            ImGui.Text("This is an open-source software, developed in C# using Dear ImGui.");
            ImGui.Text("You are welcome to use and replicate it in accordance with its usage terms.");

            ImGui.BulletText("Github: www.github.com/guillaC");
            ImGui.BulletText("E-Mail: k0odxblrd@mozmail.com");
            
            ImGui.Separator();
            ImGui.Text("Feel free to reach out for any inquiries, suggestions, or feedback !");
            
            ImGui.SeparatorText("Disclaimer");
            ImGui.Text("This software solution is intended for forensic purposes and is designed to\n" +
                "facilitate the retrieval of data from SQLite files.");
            ImGui.Text("I take no responsibility for how this program is used.");
            
            ImGui.SameLine();

            ImGui.SetCursorPosX(ImGui.GetWindowSize().X - 45);
            if (ImGui.Button("Exit"))
            {
                isOpen = false;
            }

            ImGui.End();
        }

    }
}
