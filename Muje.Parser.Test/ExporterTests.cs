using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Muje.Parser;
using NUnit;
using NUnit.Framework;

namespace Muje.Parser.Test
{
    [TestFixture]
    public class ExporterTests
    {
        [Test]
        public void ExportBussinessCard()
        {
            int start = 3101;
            int end = 3115;

            //AppDomain.CurrentDomain.ExecuteAssembly
            //Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            start = Convert.ToInt32(config.AppSettings.Settings["TraderStart"].Value);
            end = Convert.ToInt32(config.AppSettings.Settings["TraderEnd"].Value);

            DateTime begin = DateTime.Now;
            TradeCard card = new TradeCard();
            Parser parser = new Parser(card);
            parser.Parse(start, end);

            DateTime finish = DateTime.Now;
            TimeSpan diff = finish - begin;

            Console.WriteLine("Finish extract {0} companies in {1}", end - start + 1, diff);
            Console.Read();
        }
    }
}