using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDiskExplorer.Model.Schema
{

    public class Column
    {
        public required string Name { get; set; }
        public required string DataType { get; set; }
        public string? Constraint { get; set; }
        public ForeignKeyInfo? ForeignKey { get; set; }
        public required bool IsPrimary { get; set; }
    }
}
