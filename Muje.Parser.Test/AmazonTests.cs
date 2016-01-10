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
        public void Parse()
        {
            AmazonParser parser = new AmazonParser("http://www.amazon.com/Best-Sellers-Books-Investing/zgbs/books/2665");
            parser.Parse();

        }
    }
}
