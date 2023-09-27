using MediaDevices; // need to read documentation, important link : https://stackoverflow.com/questions/55208379/access-phone-files-via-net

namespace SQLiteDiskExplorer.Utils
{
    public static class Device
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Valider la compatibilité de la plateforme", Justification = "<En attente>")]
        public static List<MediaDevice> GetDevices()
        {
            var devices = MediaDevice.GetPrivateDevices().Where(d => d.DeviceType is DeviceType.Phone or DeviceType.Camera).ToList(); //GetDevices
            return devices;
        }
    }
}
