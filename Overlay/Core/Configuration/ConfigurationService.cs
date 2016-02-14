using Overlay.Core.Configuration.Model;

namespace Overlay.Core.Configuration
{
    class ConfigurationService : IConfigurationService
    {
        public Layout GetActiveLayout()
        {
            return new Layout()
            {
                Areas =
                {
                    new Column()
                    {
                        Name = "Left",
                        Width = new Measurement
                        {
                            Unit = MeasurementUnit.Percentage,
                            Value = 25
                        }
                    },
                    new Column()
                    {
                        Name = "Center",
                        Width = new Measurement
                        {
                            Unit = MeasurementUnit.Percentage,
                            Value = 50
                        }
                    },
                    new Column()
                    {
                        Name = "Right",
                        Width = new Measurement
                        {
                            Unit = MeasurementUnit.Percentage,
                            Value = 25
                        }
                    },
                }
            };
        }

        public OverlayVisuals GetOverlayVisuals()
        {
            return new OverlayVisuals()
            {
                Border = new SolidColor("#FFF14200"),
                Fill = new SolidColor("#FFFB7E02")
            };
        }
    }
}