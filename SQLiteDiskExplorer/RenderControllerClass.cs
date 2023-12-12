using ClickableTransparentOverlay;
using ImGuiNET;
using SQLiteDiskExplorer.UI;
using System.Drawing;
using System.Numerics;
using System;
using System.Runtime.InteropServices;

namespace SQLiteDiskExplorer
{
    public class RenderControllerClass : Overlay
    {
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hwnd, IntPtr dc);

        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        const int HORZRES = 8; // Horizontal Resolution
        const int VERTRES = 10; // Vertical Resolution

        bool firstLoad = true;

        public static AboutUI? aboutForm;
        public static MainUI? mainForm;
        public static ScanUI? scanForm;
        public static ConfigurationUI? configForm;
        public static SQLInfoUI? infoForm;
        public static HexUI? hexUIForm;

        protected override void Render() // loop
        {
            if (firstLoad)
            {
                ResizeOverlay();
                mainForm = new(this);
                firstLoad = !firstLoad;
            }

            mainForm!.Show();

            if (aboutForm != null) aboutForm.Show();
            if (scanForm != null) scanForm.Show();
            if (configForm != null) configForm.Show();
            if (infoForm != null) infoForm.Show();
            if (hexUIForm != null) hexUIForm.Show();
        }

        private void ResizeOverlay()
        {
            IntPtr desktopHwnd = GetDesktopWindow();
            IntPtr desktopDC = GetDC(desktopHwnd);
            int screenWidth = GetDeviceCaps(desktopDC, HORZRES);
            int screenHeight = GetDeviceCaps(desktopDC, VERTRES);
            ReleaseDC(desktopHwnd, desktopDC);
            Size = new Size(screenWidth, screenHeight);
        }
    }
}
