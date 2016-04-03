using System;
using System.Xml.Serialization;

namespace Overlay.Core.Configuration.Model
{
    [Serializable]
    public class Area
    {
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("col")]
        public int ColumnIndex { get; set; }

        [XmlAttribute("row")]
        public int RowIndex { get; set; }
    }
}