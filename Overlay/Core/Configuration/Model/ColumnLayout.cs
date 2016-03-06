using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Overlay.Core.Configuration.Model
{
    [Serializable]
    public class ColumnLayout : Layout
    {
        public List<Column> Columns { get; set; }

        public ColumnLayout()
        {
            Columns = new List<Column>();
        }
    }
}