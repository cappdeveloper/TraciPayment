using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class PersonModel
    {
        public int PersonKey { get; set; }
        public string PersonName { get; set; }
        public string PersonAddress { get; set; }

        public string PersonCity { get; set; }
        public int? PersonStateKey { get; set; }
        public string PersonZipCode { get; set; }
        public string PersonNote { get; set; }
        public string VendorFederalEIN { get; set; }
        public string VendorDefaultTerms { get; set; }
    }
}