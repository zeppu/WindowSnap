using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Snapinator.Core.Configuration.Model
{
    [Serializable]
    [XmlType("row")]
    [XmlInclude(typeof(Row))]
    public class RowLayout : Layout
    {
        public List<Row> Rows { get; set; }

        public RowLayout()
        {
            Rows = new List<Row>();
        }

    }
}