using System;
using System.Xml.Serialization;

namespace Overlay.Core.Configuration.Model
{
    [Serializable]    
    public class Column : Area
    {
        [XmlAttribute("width")]
        public string WidthAsString
        {
            get { return Width.ToString(); }
            set { Width = (Measurement)value; }
        }

        [XmlIgnore]
        public Measurement Width { get; set; }
    }
}