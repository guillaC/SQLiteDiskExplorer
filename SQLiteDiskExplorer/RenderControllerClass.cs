using ClickableTransparentOverlay;
using SQLiteDiskExplorer.UI;

namespace SQLiteDiskExplorer
{
    public class RenderControllerClass : Overlay
    {
        bool firstLoad = true;

        public AboutUI aboutForm;
        public MainUI mainForm;
        public ScanUI scanForm;
        public ConfigurationUI configForm;

        protected override void Render() // loop
        {
            if (firstLoad)
            {
                mainForm = new(this);
                firstLoad = !firstLoad;
            }


            mainForm.Show();

            if (aboutForm != null) aboutForm.Show();
            if (scanForm != null) scanForm.Show();
            if (configForm != null) configForm.Show();
        }
    }
}
