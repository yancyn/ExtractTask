using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Muje.Magnum.Parser;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime begin = DateTime.Now;
            DateTime finish = DateTime.Now;
            int start = 3100;
            int end = 3160;
            if (args.Length > 0)
            {
                Console.WriteLine(args.Length);
                string url = "http://edirectory.matrade.gov.my/application/edirectory.nsf/PrintCompany?OpenAgent&Option=detail&param=";
                start = Convert.ToInt32(args[0]);
                end = Convert.ToInt32(args[1]);
                BusinessCardParser parser = new BusinessCardParser(url, start, end);
                parser.Parse();
            }
            else
            {
                // debug case
                BusinessCardParser parser2 = new BusinessCardParser("http://edirectory.matrade.gov.my/application/edirectory.nsf/PrintCompany?OpenAgent&Option=detail&param=", start, end);
                parser2.Parse();
            }
            finish = DateTime.Now;

            TimeSpan diff = finish - begin;
            Console.WriteLine("Done extract {0} companies in {1}", end - start + 1, diff);
            Console.Read();
        }
    }
}