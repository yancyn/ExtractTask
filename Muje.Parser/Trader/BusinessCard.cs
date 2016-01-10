using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Muje.Parser.MalaysiaExporter
{
    public class BusinessCard
    {
        public string Name { get; set; }
        public string BusinessAddress { get; set; }
        public string FactoryAddress { get; set; }
        public string Telephone { get; set; }
        public string Facsimile { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string RegistrationNo { get; set; }
        public string IncorporationDate { get; set; }
        public string TypeOfBusiness { get; set; }
        public string BusinessEnquiryContact { get; set; }
        public string Designation { get; set; }
        public string BusinessEnquiryContact2 { get; set; }
        public string Designation2 { get; set; }
        public string Certification { get; set; }
        public string Products { get; set; }

        /// <summary>
        /// Override to export csv format.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string format = string.Empty;
            for (int i = 0; i < 16; i++)
            {
                if (format.Length > 0) format += ",";
                format += "\"{" + i.ToString() + "}\"";
            }

            return string.Format(format, Name, BusinessAddress, FactoryAddress, Telephone, Facsimile, Email, Website, RegistrationNo, IncorporationDate, TypeOfBusiness, BusinessEnquiryContact, Designation, BusinessEnquiryContact2, Designation2, Certification, Products);
        }
    }
}