using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Snapinator.Core.Configuration.Model
{
    [Serializable]
    [XmlRoot("snapinator")]
    [XmlInclude(typeof(ColumnLayout))]
    [XmlInclude(typeof(BooleanSetting))]
    [XmlInclude(typeof(StringSetting))]
    public class ConfigurationFile
    {
        private List<Layout> _layouts = new List<Layout>();
        private List<Setting> _interface = new List<Setting>();

        public List<Layout> Layouts
        {
            get { return _layouts; }
            set { _layouts = value; }
        }

        public List<Setting> Interface
        {
            get { return _interface; }
            set { _interface = value; }
        }
    }
}