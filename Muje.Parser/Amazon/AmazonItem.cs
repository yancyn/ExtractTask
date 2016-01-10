using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muje.Parser.Amazon
{
    public class AmazonItem
    {
        private string title;
        public string Title { get { return this.title; } }

        private string url;
        public string Url { get { return this.url; } }

        private string series;
        public string Series
        {
            get
            {
                this.series = RegexHelper.GrabPattern(this.url, "/dp/", "/ref=");
                return this.series;
            }
        }

        public AmazonItem(string title, string url)
        {
            this.title = title;
            this.url = url;
        }
        public override string ToString()
        {
            return string.Format("{0}: {1}", this.title, this.url);
        }
    }
}