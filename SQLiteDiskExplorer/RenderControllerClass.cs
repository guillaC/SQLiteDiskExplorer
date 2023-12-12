using ClickableTransparentOverlay;
using SQLiteDiskExplorer.UI;
using System.Drawing;

namespace SQLiteDiskExplorer
{
    public class RenderControllerClass : Overlay
    {
        bool firstLoad = true;

        public static AboutUI? aboutForm;
        public static MainUI? mainForm;
        public static ScanUI? scanForm;
        public static ConfigurationUI? configForm;
        public static SQLInfoUI? infoForm;
        public static HexUI? hexUIForm;

        protected override void Render() // loop
        {

            this.Size = new Size(5000, 5000);

            if (firstLoad)
            {
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
    }
}
