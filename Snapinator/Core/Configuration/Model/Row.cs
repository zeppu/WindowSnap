using System;
using System.Xml.Serialization;

namespace Overlay.Core.Configuration.Model
{

    [Serializable]
    public class Row : Area
    {
        [XmlAttribute("height")]
        public string HeightAsString
        {
            get { return Height.ToString(); }
            set { Height = (Measurement)value; }
        }
        [XmlIgnore]
        public Measurement Height { get; set; }
    }
}