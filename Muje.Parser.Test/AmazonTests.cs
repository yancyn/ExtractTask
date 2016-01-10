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
        public void Parse()
        {
            AmazonParser parser = new AmazonParser(@"http://www.amazon.com/Best-Sellers-Books-Investing/zgbs/books/2665");
            parser.Parse();
            parser.ListAffiliate();
        }
    }
}