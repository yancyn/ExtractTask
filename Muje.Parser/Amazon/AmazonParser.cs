using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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

        public AmazonParser(string url)
        {
            this.baseUrl = url;
            this.items = new List<AmazonItem>();
            this.categories = new Dictionary<string, string>();
        }
        private Dictionary<string, string> categories;
        /// <summary>
        /// Return categories.
        /// </summary>
        public Dictionary<string, string> GetCategories()
        {
            this.categories = new Dictionary<string, string>();

            string extraction = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(baseUrl);
            System.Diagnostics.Debug.WriteLine("Start parsing " + baseUrl);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string line;
                    bool start = false;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Contains("<ul id=\"zg_browseRoot\">")) start = true;
                        if (start)
                        {
                            extraction += line;
                            if (line.Contains("</ul>")) break;
                        }
                    }
                }
            }

            // parse into li collection
            MatchCollection matches = Regex.Matches(extraction, "<li>(.*?)</li>");
            for (int i = 1; i < matches.Count; i++)
            {
                string li = matches[i].Groups[0].Value;
                string category = string.Empty;
                string url = string.Empty;
                //System.Diagnostics.Debug.WriteLine(li);

                li = li.TrimStart(new char[] { '<', 'l', 'i', '>', '<', 'a', });
                li = li.TrimEnd(new char[] { '<', '/', 'a', '>', '<', '/', 'l', 'i', '>' });
                li = li.Trim();
                //System.Diagnostics.Debug.WriteLine(li);
                li = li.Replace("'>", ";"); // HACK: Replace '> with ; then split again with ;
                string[] pieces = li.Split(new char[] { ';' });
                if (pieces.Length > 0) url = pieces[0].Replace("href='", string.Empty);
                if (pieces.Length > 1) category = pieces[1];
                //System.Diagnostics.Debug.WriteLine(string.Format("{0}:{1}", category, url));
                this.categories.Add(category, url);
            }

            return this.categories;
        }

        public void ParseAll()
        {
            GetCategories();
            Parse();

            foreach (KeyValuePair<string, string> pair in this.categories)
            {
                System.Diagnostics.Debug.WriteLine("Extracting category: " + pair.Key);
                Parse(pair.Value);
            }
        }
        public void Parse()
        {
            Parse(baseUrl);
        }
        /// <summary>
        /// Extract to collection of AmazonItem based on base url.
        /// </summary>
        public void Parse(string url)
        {
            for (int i = 0; i < 5; i++)
            {
                string link = string.Format(url + "/ref=zg_bs_2665_pg_{0}?_encoding=UTF8&pg={0}", i + 1);
                System.Diagnostics.Debug.WriteLine("Start parsing " + link);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(link);
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
                                    string title = RegexHelper.GrabPattern(useful, "title=\"", "\" onload");
                                    if (string.IsNullOrEmpty(title)) title = RegexHelper.GrabPattern(useful, "title=\"", "\"/>");
                                    string u = RegexHelper.GrabPattern(useful, "href=\"", "\"><img");
                                    AmazonItem item = new AmazonItem(title, u);
                                    this.items.Add(item);
                                    System.Diagnostics.Debug.WriteLine(item.ToString());

                                    start = false;
                                    useful = string.Empty;
                                }
                            }
                        }
                    }
                }// close http request
            }// end loop
        }
        /// <summary>
        /// Generate collection of urls can be use in Amazonaire.
        /// </summary>
        public void ListAffiliate(string url)
        {
            foreach (AmazonItem item in items)
                System.Diagnostics.Debug.WriteLine(url + item.Series);
        }
    }
}