using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ExtractBusinessCard
{
    public class ArrayCard: CardFactory
    {
        private List<List<string>> lines;
        public override void Extract(string uri)
        {
            lines = new List<List<string>>();
            System.Diagnostics.Debug.WriteLine("Uri:" + uri);
            Console.WriteLine("Parsing " + uri);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string html = reader.ReadToEnd();
                    html = Replace(html);
                    MatchCollection matches = Regex.Matches(html, "<table border=0 width=100% cellspacing=3>.*?</table>", RegexOptions.Multiline);
                    foreach (Match match in matches)
                    {
                        foreach (Group group in match.Groups)
                        {
                            // TODO: Continue extract table row
                            System.Diagnostics.Debug.WriteLine(group.ToString());
                            List<string> line = GetCells(group.ToString());
                            this.lines.Add(line);
                        }
                    }
                }
            }
        }
        private List<string> GetCells(string line)
        {
            List<string> record = new List<string>();
            MatchCollection matches = Regex.Matches(line, "(?<=^|>)[^><]+?(?=<|$)");
            foreach (Match match in matches)
            {
                foreach (Group group in match.Groups)
                    record.Add(group.Value);
            }
            return record;
        }
        /// <summary>
        /// Replace some exception character to match regex result.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private string Replace(string source)
        {
            string result = source;
            result = result.Replace("\n", "");
            result = result.Replace("<br>", " ");
            return result;
        }

        public override void Process()
        {
            StreamWriter writer = new StreamWriter(OUTPUT);
            string format = string.Empty;
            for (int i = 0; i < 16; i++)
            {
                if (format.Length > 0) format += ",";
                format += "\"{" + i.ToString() + "}\"";
            }
            writer.WriteLine(string.Format(format, "Name", "BusinessAddress", "FactoryAddress", "Telephone", "Facsimile", "Email", "Website", "RegistrationNo", "IncorporationDate", "TypeOfBusiness", "BusinessEnquiryContact", "Designation", "BusinessEnquiryContact", "Designation", "Certification", "Products"));
            foreach (List<string> line in this.lines)
            {
                string f = string.Empty;
                foreach (string cell in line)
                {
                    if (f.Length > 0) f += ",";
                    f += "\"" + cell + "\"";
                }
                writer.WriteLine(f);
            }
            writer.Close();
        }
    }
}
