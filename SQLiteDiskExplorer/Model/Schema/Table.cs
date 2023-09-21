using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteDiskExplorer.Model.Schema
{
    public class Table
    {
        public required string TableName { get; set; }
        public required List<Column> Columns { get; set; }
    }
}
