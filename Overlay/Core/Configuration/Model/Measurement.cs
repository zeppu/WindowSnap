using System;
using System.Windows.Forms;

namespace Overlay.Core.Configuration.Model
{
    [Serializable]
    public class Measurement
    {
        public double Value { get; set; }

        public MeasurementUnit Unit { get; set; }

        public static implicit operator double(Measurement m)
        {
            switch (m.Unit)
            {
                case MeasurementUnit.Percentage:
                    return ((m.Value / 100) * Screen.PrimaryScreen.Bounds.Width);
                    break;
                case MeasurementUnit.Pixels:
                    return m.Value;
                    break;
            }

            throw new InvalidOperationException();
        }
    }

    public enum MeasurementUnit
    {
        Percentage,
        Pixels,
    }
}