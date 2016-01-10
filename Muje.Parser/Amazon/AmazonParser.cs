using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
            this.items = new List<AmazonItem>();
            this.categories = new Dictionary<string, string>();
        }
        /// <summary>
        /// Extract to collection of AmazonItem.
        /// </summary>
        public void Parse()
        {
            for (int i = 0; i < 5; i++)
            {
                string link = baseUrl + "#" + (i + 1).ToString();
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);
                System.Diagnostics.Debug.WriteLine("Start parsing " + link);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        string line;
                        bool start = false;
                        string useful = string.Empty;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line.Contains("<div class=\"zg_itemImageImmersion\">")) start = true;
                            if (start)
                            {
                                useful += line;
                                if (line.Contains("<img src="))
                                {
                                    start = false;

                                    string title = RegexHelper.GrabPattern(useful, "title=\"", "\" onload");
                                    if (string.IsNullOrEmpty(title)) title = RegexHelper.GrabPattern(useful, "title=\"", "\"/>");
                                    string url = RegexHelper.GrabPattern(useful, "href=\"", "\"><img");
                                    AmazonItem item = new AmazonItem(title, url);
                                    this.items.Add(item);
                                    System.Diagnostics.Debug.WriteLine(item.ToString());

                                    useful = string.Empty;
                                }
                            }
                        }
                    }
                }// close http request
            }// end loop
            System.Diagnostics.Debug.WriteLine("Result found: " + items.Count);
        }
        /// <summary>
        /// Generate collection of urls can be use in Amazonaire.
        /// </summary>
        public void ListAffiliate()
        {

        }
    }
}