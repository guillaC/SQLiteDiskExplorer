using ImGuiNET;
using System.Text;

namespace SQLiteDiskExplorer.Model
{
    public class SQLiteFileHeader //https://www.sqlite.org/fileformat.html
    {
        public byte[] Header { get; set; }
        public ushort PageSize { get; set; }
        public byte FileFormatWriteVersion { get; set; }
        public byte FileFormatReadVersion { get; set; }
        public uint FileChangeCounter { get; set; }
        public uint DatabaseSizeInPages { get; set; }
        public uint FirstFreelistTrunkPage { get; set; }
        public uint TotalFreelistPages { get; set; }
        public uint SchemaCookie { get; set; }
        public uint SchemaFormatNumber { get; set; }
        public uint DefaultPageCacheSize { get; set; }
        public uint UserVersion { get; set; }
        public uint VersionValidForNumber { get; set; }
        public uint SQLiteVersionNumber { get; set; }

        public SQLiteFileHeader(byte[] header)
        {
            Header = header;

            PageSize = BitConverter.ToUInt16(header, 16);
            FileFormatWriteVersion = header[18];
            FileFormatReadVersion = header[19];
            FileChangeCounter = BitConverter.ToUInt32(header, 24);
            DatabaseSizeInPages = BitConverter.ToUInt32(header, 28);
            FirstFreelistTrunkPage = BitConverter.ToUInt32(header, 32);
            TotalFreelistPages = BitConverter.ToUInt32(header, 36);
            SchemaCookie = BitConverter.ToUInt32(header, 40);
            SchemaFormatNumber = BitConverter.ToUInt32(header, 44);
            DefaultPageCacheSize = BitConverter.ToUInt32(header, 48);
            UserVersion = BitConverter.ToUInt32(header, 60);
            VersionValidForNumber = BitConverter.ToUInt32(header, 92);
            SQLiteVersionNumber = BitConverter.ToUInt32(header, 96);
        }
    }
}
