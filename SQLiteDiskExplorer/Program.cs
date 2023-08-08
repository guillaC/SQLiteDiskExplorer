using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Windowing.Desktop;

namespace SQLiteDiskExplorer
{
    class Program
    {
        /// <summary>
        /// Cette application utilise un template développé par : https://github.com/NogginBops/ImGui.NET_OpenTK_Sample
        /// Il utilise les librairies OpenTK & ImGui.NET.
        /// </summary>
        static void Main()
        {
            Window wnd = new Window();
            wnd.Title = "SQLiteDiskExplorer";
            wnd.Run();
        }
    }
}
