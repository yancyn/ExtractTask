using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Muje.Parser.Amazon
{
    public class AmazonParser
    {
        /// <summary>
        /// Base Amazon url to start extraction.
        /// </summary>
        private string baseUrl;

        private List<AmazonItem> items;
        /// <summary>
        /// Returns collection of items found based on base url.
        /// </summary>
        public AmazonItem[] Result { get { return this.items.ToArray(); } }

        private Dictionary<string, string> categories;
        /// <summary>
        /// Return categories.
        /// </summary>
        public Dictionary<string, string> Catagories { get { return this.categories; } }

        public AmazonParser(string url)
        {
            this.baseUrl = url;
            this.categories = new Dictionary<string, string>();
        }
        public void Parse()
        {

        }
    }
}