using System.Xml.Serialization;

namespace Snapinator.Core.Configuration.Model
{
    public abstract class Setting
    {
        [XmlAttribute]
        public string Id { get; set; }

        public abstract object GetValue();

        public abstract void SetValue(object value);
    }

    [XmlType("bool")]
    public class BooleanSetting : Setting
    {
        public BooleanSetting(string id, bool value)
        {
            Value = value;
            Id = id;
        }

        public BooleanSetting()
        {

        }

        [XmlAttribute]
        public bool Value { get; set; }

        public override object GetValue()
        {
            return Value;
        }

        public override void SetValue(object value)
        {
            Value = (bool)value;
        }
    }

    [XmlType("string")]
    public class StringSetting : Setting
    {
        public StringSetting(string id, string value)
        {
            Value = value;
            Id = id;
        }

        public StringSetting()
        {

        }

        [XmlText]
        public string Value { get; set; }

        public override object GetValue()
        {
            return Value;
        }

        public override void SetValue(object value)
        {
            Value = (string)value;
        }
    }
}