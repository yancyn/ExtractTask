using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Muje.Parser.MalaysiaExporter
{
    public class TraderParser
    {
        private const int bulk = 15;
        private const string URI = "http://edirectory.matrade.gov.my/application/edirectory.nsf/PrintCompany?OpenAgent&Option=detail&param=";
        private CardFactory card;
        public TraderParser(CardFactory card)
        {
            this.card = card;
        }
        public void Parse(int start, int end)
        {
            string address = URI;
            for (int i = start; i <= end; i++)
            {
                if ((i - start) % bulk == 0)
                {
                    if (i > start) this.card.Extract(address);
                    address = URI;//reset
                }
                if ((i - start) % bulk > 0) address += "+";
                address += i.ToString();
            }
            this.card.Extract(address);

            this.card.Process();
        }
    }
}