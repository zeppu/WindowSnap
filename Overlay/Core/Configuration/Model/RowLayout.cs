using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Overlay.Core.Configuration.Model
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