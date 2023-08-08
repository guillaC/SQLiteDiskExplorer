using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDiskExplorer.Utils
{
    public static class Drive
    {
        public static List<DriveInfo> GetDrives()
        {
            var drives = DriveInfo.GetDrives().ToList();
            return drives;
        }

        public static string GetDriveTypeDescription(DriveType driveType)
        {
            switch (driveType)
            {
                case DriveType.CDRom:
                    return "CD/DVD-ROM Drive";
                case DriveType.Fixed:
                    return "Local Hard Drive";
                case DriveType.Network:
                    return "Network Drive";
                case DriveType.Removable:
                    return "Removable Drive";
                case DriveType.Ram:
                    return "RAM Disk";
                case DriveType.Unknown:
                default:
                    return "Unknown Drive Type";
            }
        }

        public static string FormatSize(long bytes) // thx to Chat GPT
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            int order = 0;
            while (bytes >= 1024 && order < sizes.Length - 1)
            {
                bytes /= 1024;
                order++;
            }
            return $"{bytes:0.##} {sizes[order]}";
        }
    }
}