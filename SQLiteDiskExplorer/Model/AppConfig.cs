using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDiskExplorer.Model
{
    public class AppConfig
    {
        public bool IgnoreInaccessible { get; set; }
        public bool RecurseSubdirectories { get; set; }
        public bool CheckPathKeywordPresence { get; set; }
        public bool CheckColumnKeywordPresence { get; set; }
        public bool CopyToTempIfOpnInAnotherProcess { get; set; }
        public required List<string> ImportantKeywords { get; set; }
    }
}
