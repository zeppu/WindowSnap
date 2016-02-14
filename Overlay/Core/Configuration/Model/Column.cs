using System;

namespace Overlay.Core.Configuration.Model
{
    [Serializable]
    public class Column : Area
    {
        public Measurement Width { get; set; }
    }
}