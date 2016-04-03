using System;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Snapinator.Core.Configuration.Model
{
    public class SolidColor
    {
        private Color _color;

        public SolidColor()
        {

        }

        public SolidColor(SolidColorBrush solidColorBrush) : this(solidColorBrush.Color)
        {
        }

        public SolidColor(Color color)
        {
            _color = color;
        }

        public SolidColor(string value)
        {
            var color = ColorConverter.ConvertFromString(value);
            if (color == null)
                throw new InvalidOperationException();

            _color = (Color)color;
        }

        public Color ToColor()
        {
            return _color;
        }

        public SolidColorBrush ToBrush()
        {
            return new SolidColorBrush(_color);
        }

        public static implicit operator Color(SolidColor solidColor)
        {
            return solidColor.ToColor();
        }

        public static implicit operator SolidColorBrush(SolidColor solidColor)
        {
            return solidColor.ToBrush();
        }

        public static implicit operator SolidColor(Color color)
        {
            return new SolidColor(color);
        }

        public static implicit operator SolidColor(SolidColorBrush brush)
        {
            return new SolidColor(brush);
        }

        [XmlAttribute]
        public string Argb
        {
            get
            {
                return $"#{_color.R:X2}{_color.G:X2}{_color.B:X2}";
            }
            set
            {
                if (value == null)
                    throw new InvalidOperationException();

                var color = ColorConverter.ConvertFromString(value);
                if (color == null)
                    throw new InvalidOperationException();

                _color = (Color)color;
            }
        }
    }
}