using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDiskExplorer.Model
{


    public class Column
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public string Constraint { get; set; }
        public ForeignKeyInfo? ForeignKey { get; set; }
        public bool IsPrimary { get; set; }
    }

    public class ForeignKeyInfo
    {
        public string ReferencedTable { get; set; }
        public string ReferencedColumn { get; set; }
    }
}
