﻿using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Snapinator.Core.Configuration.Model
{
    [Serializable]
    [XmlInclude(typeof(ColumnLayout))]
    [XmlInclude(typeof(RowLayout))]
    public class Layout
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("displayName")]
        public string DisplayName { get; set; }

        [XmlAttribute("active")]
        public bool IsActive { get; set; }

        public List<TargetScreen> ActiveScreens { get; set; }
    }

    [Serializable]
    public class TargetScreen
    {
        [XmlAttribute]
        public int ScreenIndex { get; set; }
    }
}