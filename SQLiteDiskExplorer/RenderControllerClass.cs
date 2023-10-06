using ClickableTransparentOverlay;
using SQLiteDiskExplorer.UI;

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

        protected override void Render() // loop
        {
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
        }
    }
}
