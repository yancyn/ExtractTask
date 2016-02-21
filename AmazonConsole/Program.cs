using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Muje.Parser;
using Muje.Parser.Amazon;

namespace AmazonConsole
{
    // source https://msdn.microsoft.com/en-us/library/hh300224.aspx
    class Program
    {
        private static List<AmazonItem> items;
        private static List<string> SetUpURLList()
        {
            var urls = new List<string>
            { 
                "http://www.amazon.com/Best-Sellers-Books-Investing/zgbs/books/2665/ref=zg_bs_2665_pg_1?_encoding=UTF8&pg=1",
                "http://www.amazon.com/Best-Sellers-Books-Investing/zgbs/books/2665/ref=zg_bs_2665_pg_2?_encoding=UTF8&pg=2",
                "http://www.amazon.com/Best-Sellers-Books-Investing/zgbs/books/2665/ref=zg_bs_2665_pg_3?_encoding=UTF8&pg=3",
                "http://www.amazon.com/Best-Sellers-Books-Investing/zgbs/books/2665/ref=zg_bs_2665_pg_4?_encoding=UTF8&pg=4",
                "http://www.amazon.com/Best-Sellers-Books-Investing/zgbs/books/2665/ref=zg_bs_2665_pg_5?_encoding=UTF8&pg=5",
            };
            return urls;
        }
        private static byte[] GetURLContents(string url)
        {
            // The downloaded resource ends up in the variable named content.
            var content = new MemoryStream();

            // Initialize an HttpWebRequest for the current URL.
            var webReq = (HttpWebRequest)WebRequest.Create(url);

            // Send the request to the Internet resource and wait for
            // the response.
            // Note: you can't use HttpWebRequest.GetResponse in a Windows Store app.
            using (WebResponse response = webReq.GetResponse())
            {
                // Get the data stream that is associated with the specified URL.
                using (Stream responseStream = response.GetResponseStream())
                {
                    // Read the bytes in responseStream and copy them to content.  
                    responseStream.CopyTo(content);
                }
            }

            // parse and add AmazonItem into collection
            string html = UTF8Encoding.UTF8.GetString(content.ToArray());
            string[] lines = html.Split(new char[] { '\n'});
            bool start = false;
            string useful = string.Empty;
            foreach(string line in lines)
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
                        items.Add(item);
                        System.Diagnostics.Debug.WriteLine(item.ToString());

                        start = false;
                        useful = string.Empty;
                    }
                }
            }

            // Return the result as a byte array.
            return content.ToArray();
        }
        private static void DisplayResults(string url, byte[] content)
        {
            // Display the length of each website. The string format 
            // is designed to be used with a monospaced font, such as
            // Lucida Console or Global Monospace.
            var bytes = content.Length;
            // Strip off the "http://".
            var displayURL = url.Replace("http://", "");
            Console.WriteLine(string.Format("\n{0,-58} {1,8}", displayURL, bytes));
        }

        static void Main(string[] args)
        {
            items = new List<AmazonItem>();

            // Make a list of web addresses.
            List<string> urlList = SetUpURLList();

            var total = 0;
            foreach (var url in urlList)
            {
                // GetURLContents returns the contents of url as a byte array.
                byte[] urlContents = GetURLContents(url);
                //string html = UTF8Encoding.UTF8.GetString(urlContents);

                DisplayResults(url, urlContents);
                //Console.WriteLine(html);

                // Update the total.
                total += urlContents.Length;
            }

            // Display the total count for all of the web addresses.
            Console.WriteLine(string.Format("\r\n\r\nTotal bytes returned:  {0}\r\n", total));
            Console.ReadKey();
        }
    }
}