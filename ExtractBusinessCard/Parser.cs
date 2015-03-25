using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtractBusinessCard
{
    public class Parser
    {
        private int start;
        private int end;
        private const int bulk = 15;
        private const string URI = "http://edirectory.matrade.gov.my/application/edirectory.nsf/PrintCompany?OpenAgent&Option=detail&param=";
        private CardFactory card;
        public Parser(CardFactory card, int start, int end)
        {
            this.card = card;
            this.start = start;
            this.end = end;
        }
        public void Parse()
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