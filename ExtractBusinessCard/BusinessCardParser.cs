using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Muje.Magnum.Parser
{
    public class BusinessCardParser
    {
        private int start;
        private int end;
        private string uri;
        private List<BusinessCard> cards;
        public List<BusinessCard> Cards;

        public BusinessCardParser(string uri, int start, int end)
        {
            this.uri = uri;
            this.start = start;
            this.end = end;
            this.cards = new List<BusinessCard>();
        }
        public void Parse()
        {
            Extract();
            Process();
        }

        /// <summary>
        /// Parse every n of business card to avoid longer url characters
        /// </summary>
        private void Extract()
        {
            int bulk = 15;
            string address = this.uri;
            for (int i = start; i <= end; i++)
            {
                if ((i - start) % bulk == 0)
                {
                    if (i > start)
                        ExtractUrl(address);
                    address = this.uri;//reset
                }
                if ((i - start) % bulk > 0) address += "+";
                address += i.ToString();
            }
            ExtractUrl(address);
        }
        private void ExtractUrl(string address)
        {
            System.Diagnostics.Debug.WriteLine("Uri:" + address);
            Console.WriteLine("Parsing " + address);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    BusinessCard card = new BusinessCard();
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        //System.Diagnostics.Debug.WriteLine(line);
                        if (line.Contains("<table border=0 width=100% cellspacing=3>"))
                        {
                            if (card.Name != null) this.cards.Add(card);
                            card = new BusinessCard();
                        }
                        if (line.Contains("Name")) card.Name = GetCellValue(line);
                        if (line.Contains("Business Address")) card.BusinessAddress = GetCellValue(line);
                        if (line.Contains("Factory Address")) card.FactoryAddress = GetCellValue(line);
                        if (line.Contains("Telephone")) card.Telephone = GetCellValue(line);
                        if (line.Contains("Facsimile")) card.Facsimile = GetCellValue(line);
                        if (line.Contains("Email")) card.Email = GetCellValue(line);
                        if (line.Contains("Website")) card.Website = GetCellValue(line);
                        if (line.Contains("Registration No")) card.RegistrationNo = GetCellValue(line);
                        if (line.Contains("Incorporation Date")) card.IncorporationDate = GetCellValue(line);
                        if (line.Contains("Type Of Business")) card.TypeOfBusiness = GetCellValue(line);
                        if (line.Contains("Business Enquiry Contact"))
                        {
                            if (card.BusinessEnquiryContact == null)
                                card.BusinessEnquiryContact = GetCellValue(line);
                            else
                                card.BusinessEnquiryContact2 = GetCellValue(line);
                        }
                        if (line.Contains("Designation"))
                        {
                            if (card.Designation == null)
                                card.Designation = GetCellValue(line);
                            else
                                card.Designation2 = GetCellValue(line);
                        }
                        if (line.Contains("Certification")) card.Certification = GetCellValue(line);
                        if (line.Contains("Product(s")) card.Products = GetCellValue(line);
                    }// end loop

                    if (card.Name != null) this.cards.Add(card);
                }
            }
        }

        /// <summary>
        /// Better extraction with Regex.
        /// </summary>
        /// <param name="address"></param>
        private void ExtractUrl2(string address)
        {
            System.Diagnostics.Debug.WriteLine("Uri:" + address);
            Console.WriteLine("Parsing " + address);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string html = reader.ReadToEnd();
                    html = html.Replace("\n", "");
                    MatchCollection matches = Regex.Matches(html, "<table border=0 width=100% cellspacing=3>.*?</table>", RegexOptions.Multiline);
                    foreach (Match match in matches)
                    {
                        foreach (Group group in match.Groups)
                        {
                            // TODO: Continue extract table row
                            System.Diagnostics.Debug.WriteLine(group.ToString());
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Extract the value from html tag.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string GetCellValue(string line)
        {
            string output = string.Empty;
            MatchCollection matches = Regex.Matches(line, "(?<=^|>)[^><]+?(?=<|$)");
            for (int i = 2; i < matches.Count; i++)
                output += matches[i].Groups[0].Value;
            return output;
        }

        private void Process()
        {
            StreamWriter writer = new StreamWriter("output.csv");

            string format = string.Empty;
            for (int i = 0; i < 16; i++)
            {
                if (format.Length > 0) format += ",";
                format += "\"{" + i.ToString() + "}\"";
            }
            writer.WriteLine(string.Format(format, "Name", "BusinessAddress", "FactoryAddress", "Telephone", "Facsimile", "Email", "Website", "RegistrationNo", "IncorporationDate", "TypeOfBusiness", "BusinessEnquiryContact", "Designation", "BusinessEnquiryContact", "Designation", "Certification", "Products"));
            foreach (BusinessCard card in this.cards)
                writer.WriteLine(card.ToString());
            writer.Close();

        }
    }
}