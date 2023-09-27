using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDiskExplorer.Model.Schema
{

    public class Column
    {
        public string Name { get; set; }
        public string? DataType { get; set; }
        public ForeignKeyInfo? ForeignKey { get; set; }
        public bool IsPrimary { get; set; }
    }
}
