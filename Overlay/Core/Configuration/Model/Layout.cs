using System;
using System.Collections.Generic;
using System.Windows.Documents;
using System.Xml;
using System.Xml.Serialization;

namespace Overlay.Core.Configuration.Model
{
    [Serializable]
    public class Layout
    {
        [XmlElement]
        public List<Area> Areas { get; private set; }

        public Layout()
        {
            Areas = new List<Area>();
        }
    }
}