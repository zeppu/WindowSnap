using System;
using System.Collections.Generic;

namespace Snapinator.Core.Configuration.Model
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