using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ExtractBusinessCard
{
    public class ObjectCard: CardFactory
    {
        private List<BusinessCard> cards;
        public override void Extract(string uri)
        {
            this.cards = new List<BusinessCard>();
            System.Diagnostics.Debug.WriteLine("Uri:" + uri);
            Console.WriteLine("Parsing " + uri);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
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
        /// Extract the value from html tag.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private string GetCellValue(string line)
        {
            string output = string.Empty;
            MatchCollection matches = Regex.Matches(line, "(?<=^|>)[^><]+?(?=<|$)");
            for (int i = 2; i < matches.Count; i++)
                output += matches[i].Groups[0].Value + " ";
            return output.Trim();
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
            foreach (BusinessCard card in this.cards)
                writer.WriteLine(card.ToString());
            writer.Close();
        }
    }
}
