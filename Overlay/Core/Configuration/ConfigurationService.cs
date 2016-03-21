using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Xml.Serialization;
using Overlay.Core.Configuration.Model;

namespace Overlay.Core.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(ConfigurationFile));

        private readonly List<Layout> _layouts = new List<Layout>();

        public Layout GetActiveLayout()
        {
            return _layouts.FirstOrDefault(l => l.IsActive);
        }

        private static Layout CreateDefault3ColumnLayout()
        {
            var layout = new ColumnLayout
            {
                IsActive = true,
                Name = "Default3Column",
                DisplayName = "3-Column"
            };

            layout.Columns.AddRange(new[]
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
            });

            return layout;
        }

        public OverlayVisuals GetOverlayVisuals()
        {
            return new OverlayVisuals()
            {
                Border = new SolidColor("#FFF14200"),
                Fill = new SolidColor("#FFFB7E02")
            };
        }

        public void LoadConfiguration()
        {
            var userappDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var dataDirectoryPath = Path.Combine(userappDataDirectory, "Snapinator");
            var dataDirectoryInfo = new DirectoryInfo(dataDirectoryPath);
            if (!dataDirectoryInfo.Exists)
            {
                dataDirectoryInfo.Create();
            }

            var configurationFileInfo = new FileInfo(Path.Combine(dataDirectoryPath, "config.xml"));
            ConfigurationFile configurationFile = null;
            if (!configurationFileInfo.Exists)
            {
                configurationFile = InitializeNewConfiguration(configurationFileInfo);
            }
            else
            {
                configurationFile = LoadConfigurationFile(configurationFileInfo);
            }

            _layouts.AddRange(configurationFile.Layouts);
        }

        public IEnumerable<Layout> GetLayouts()
        {
            return _layouts;
        }

        public ConfigurationFile LoadConfigurationFile(FileInfo layoutsFile)
        {
            ConfigurationFile configFile;
            using (var f = layoutsFile.OpenRead())
            {
                configFile = (ConfigurationFile)Serializer.Deserialize(f);
                f.Flush();
            }

            return configFile;
        }

        public ConfigurationFile InitializeNewConfiguration(FileInfo layoutsFile)
        {
            using (var f = layoutsFile.OpenWrite())
            using (var writer = new StreamWriter(f))
            {
                return InitializeNewConfiguration(writer);
                writer.Flush();
            }
        }
        public ConfigurationFile InitializeNewConfiguration(TextWriter layoutsFile)
        {
            var configFile = new ConfigurationFile()
            {
                Layouts =
                {
                    (ColumnLayout)CreateDefault3ColumnLayout()
                }
            };

            Serializer.Serialize(layoutsFile, configFile);

            return configFile;
        }
    }
}