using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Muje.Parser.Amazon;
using NUnit;
using NUnit.Framework;

namespace Muje.Parser.Test
{
    [TestFixture]
    public class AmazonTests
    {
        [Test]
        public void SeriesTest()
        {
            AmazonItem target = new AmazonItem("The Index Card: Why Personal Finance Doesn’t Have to Be Complicated", @"http://www.amazon.com/The-Index-Card-Personal-Complicated/dp/1591847680/ref=zg_bs_2665_1");
            string expected = "1591847680";
            string actual = target.Series;
            Console.WriteLine(actual);
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void AmazonItemTest()
        {
            AmazonItem target = new AmazonItem("The Index Card: Why Personal Finance Doesn’t Have to Be Complicated", @"http://www.amazon.com/The-Index-Card-Personal-Complicated/dp/1591847680/ref=zg_bs_2665_1");
            string expected = "The Index Card: Why Personal Finance Doesn’t Have to Be Complicated";
            string actual = target.Title;
            Console.WriteLine(actual);
            Assert.AreEqual(expected, actual);

            expected = @"http://www.amazon.com/The-Index-Card-Personal-Complicated/dp/1591847680/ref=zg_bs_2665_1";
            actual = target.Url;
            Console.WriteLine(actual);
            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void ListSplitTest()
        {
            string target = @"href='http://www.amazon.com/Best-Sellers-Books-Investment-Analysis-Strategy/zgbs/books/10020703011/ref=zg_bs_nav_b_3_2665/175-9033810-2540516'>Analysis & Strategy";
            target = target.Replace("'>", ";");
            string[] pieces = target.Split(new char[] { ';' });
            string category = string.Empty;
            foreach (string piece in pieces)
            {
                System.Diagnostics.Debug.WriteLine(piece);
                category = piece;
            }
            Assert.AreEqual("Analysis & Strategy", category);
        }
        [Test]
        public void GetCategoriesTest()
        {
            AmazonParser parser = new AmazonParser(@"http://www.amazon.com/Best-Sellers-Books-Investing/zgbs/books/2665");
            string[] expecteds = new string[] { 
                "Analysis & Strategy",
                "Bonds",
                "Commodities",
                "Futures",
                "Introduction",
                "Mutual Funds",
                "Online Trading",
                "Options",
                "Real Estate",
                "Stocks"
            };
            int i=0;
            foreach (KeyValuePair<string, string> pair in parser.GetCategories())
            {
                System.Diagnostics.Debug.WriteLine("{0}:{1}", pair.Key, pair.Value);
                Assert.AreEqual(expecteds[i], pair.Key);
                i++;
            }
        }
        [Test]
        public void Parse()
        {
            AmazonParser parser = new AmazonParser(@"http://www.amazon.com/Best-Sellers-Books-Investing/zgbs/books/2665");
            parser.Parse();
            parser.ListAffiliate();
        }
    }
}