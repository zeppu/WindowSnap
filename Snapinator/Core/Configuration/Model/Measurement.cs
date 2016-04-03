using System;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Overlay.Core.Configuration.Model
{
    [Serializable]
    public class Measurement
    {
        public double Value { get; set; }

        public MeasurementUnit Unit { get; set; }

        public override string ToString()
        {
            switch (Unit)
            {
                case MeasurementUnit.Percentage:
                    return $"{Value:F}%";
                case MeasurementUnit.Pixels:
                    return $"{Value:N}px";
            }

            return base.ToString();
        }

        public static explicit operator Measurement(string v)
        {
            if (v.EndsWith("%"))
            {
                var value = v.Substring(0, v.Length - 1);
                return new Measurement()
                {
                    Value = Convert.ToDouble(value),
                    Unit = MeasurementUnit.Percentage
                };
            }

            if (v.EndsWith("px"))
            {
                var value = v.Substring(0, v.Length - 2);
                return new Measurement()
                {
                    Value = Convert.ToDouble(value),
                    Unit = MeasurementUnit.Pixels
                };
            }

            throw new InvalidOperationException();
        }
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            var x = reader.Value;
        }

        public void WriteXml(XmlWriter writer)
        {

        }

        public Measurement()
        {

        }

        public Measurement(double value, MeasurementUnit unit)
        {
            Value = value;
            Unit = unit;
        }
    }

    public enum MeasurementUnit
    {
        Percentage,
        Pixels,
    }

    public static class MeasurementFactory
    {
        public static Measurement FromString(string value)
        {
            return (Measurement)value;
        }

        public static Measurement FromPercentage(double percentage)
        {
            return new Measurement()
            {
                Value = percentage,
                Unit = MeasurementUnit.Percentage
            };
        }
        public static Measurement FromPixels(int pixels)
        {
            return new Measurement()
            {
                Value = pixels,
                Unit = MeasurementUnit.Pixels
            };
        }
    }
}