using System;
using System.Windows.Forms;

namespace Overlay.Core.Configuration.Model
{
    [Serializable]
    public class Measurement
    {
        public double Value { get; set; }

        public MeasurementUnit Unit { get; set; }
    }

    public enum MeasurementUnit
    {
        Percentage,
        Pixels,
    }
}