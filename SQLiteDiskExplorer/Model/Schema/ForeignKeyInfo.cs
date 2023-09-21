using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDiskExplorer.Model.Schema
{
    public class ForeignKeyInfo
    {
        public required string ReferencedTable { get; set; }
        public required string ReferencedColumn { get; set; }
    }
}
