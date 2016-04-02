using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Overlay.Core.Configuration;

namespace Overlay.Tests.Core.Configuration
{
    [TestClass]
    public class ConfigurationServiceTests
    {
        [TestMethod]
        public void TestConfigurationFileOutput()
        {
            var cs = new ConfigurationService();
            cs.InitializeNewConfiguration(Console.Out);
        }
    }
}
