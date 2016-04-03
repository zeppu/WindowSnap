using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Snapinator.Core.Configuration.Model
{
    [Serializable]
    [XmlRoot("snapinator")]
    [XmlInclude(typeof(ColumnLayout))]
    public class ConfigurationFile
    {
        private List<Layout> _layouts = new List<Layout>();

        public List<Layout> Layouts
        {
            get { return _layouts; }
            set { _layouts = value; }
        }
    }
}