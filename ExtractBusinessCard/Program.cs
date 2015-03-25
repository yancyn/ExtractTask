using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtractBusinessCard
{
    class Program
    {
        static void Main(string[] args)
        {
            int start = 3101;
            int end = 3115;
            if (args.Length > 0)
            {
                start = Convert.ToInt32(args[0]);
                end = Convert.ToInt32(args[1]);
            }

            DateTime begin = DateTime.Now;

            ObjectCard card = new ObjectCard();
            Parser parser = new Parser(card, start, end);
            parser.Parse();

            DateTime finish = DateTime.Now;
            TimeSpan diff = finish - begin;

            Console.WriteLine("Finish extract {0} companies in {1}", end - start + 1, diff);
            Console.Read();
        }
    }
}