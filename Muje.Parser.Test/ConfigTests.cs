using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit;
using NUnit.Framework;

namespace Muje.Parser.Test
{
    [TestFixture]
    public class ConfigTests
    {
        [Test]
        public void ReadAppSettingsValueTest()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            string actual = config.AppSettings.Settings["TraderStart"].Value;
            Console.WriteLine("TraderStart: " + actual);
            Assert.AreNotEqual(string.Empty, actual);

            actual = config.AppSettings.Settings["TraderEnd"].Value;
            Console.WriteLine("TraderEnd: " + actual);
            Assert.AreNotEqual(string.Empty, actual);
        }
    }
}