using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Snapinator.Core.Configuration.Model;

namespace Snapinator.Core.Configuration
{
    public class ConfigurationService : IConfigurationService
    {
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(ConfigurationFile));
        private readonly List<Layout> _layouts = new List<Layout>();

        public ConfigurationService()
        {

        }


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
            var configurationFileInfo = GetConfigurationFileInfo();
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

        private static FileInfo GetConfigurationFileInfo()
        {
            var dataDirectoryPath = GetDataDirectoryPath();

            var configurationFileInfo = new FileInfo(Path.Combine(dataDirectoryPath, "config.xml"));
            return configurationFileInfo;
        }

        private static string GetDataDirectoryPath()
        {
            var userappDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var dataDirectoryPath = Path.Combine(userappDataDirectory, "Snapinator");
            var dataDirectoryInfo = new DirectoryInfo(dataDirectoryPath);
            if (!dataDirectoryInfo.Exists)
            {
                dataDirectoryInfo.Create();
            }

            return dataDirectoryPath;
        }

        public void SaveConfiguration()
        {
            var configurationFileInfo = GetConfigurationFileInfo();
            var configurationFile = new ConfigurationFile();
            configurationFile.Layouts.AddRange(_layouts);

            SaveConfigurationFile(configurationFile, configurationFileInfo);
        }

        public IEnumerable<Layout> GetLayouts()
        {
            return _layouts;
        }

        public void AddLayout(Layout newLayout)
        {
            _layouts.Add(newLayout);
        }

        public Layout GetLayoutByName(string originalName)
        {
            return _layouts.First(layout => layout.Name.Equals(originalName));
        }

        internal void SaveConfigurationFile(ConfigurationFile configurationFile, FileInfo configurationFileInfo)
        {
            using (var f = configurationFileInfo.Open(FileMode.Create, FileAccess.Write, FileShare.Read))
            using (var writer = new StreamWriter(f))
            {
                Serializer.Serialize(writer, configurationFile);
                writer.Flush();
            }
        }

        internal ConfigurationFile LoadConfigurationFile(FileInfo configurationFileInfo)
        {
            ConfigurationFile configFile;
            using (var f = configurationFileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                configFile = (ConfigurationFile)Serializer.Deserialize(f);
                f.Flush();
            }

            return configFile;
        }

        internal ConfigurationFile InitializeNewConfiguration(FileInfo configurationFileInfo)
        {
            var configurationFile = new ConfigurationFile()
            {
                Layouts =
                {
                    (ColumnLayout)CreateDefault3ColumnLayout()
                }
            };

            SaveConfigurationFile(configurationFile, configurationFileInfo);

            return configurationFile;
        }
    }
}