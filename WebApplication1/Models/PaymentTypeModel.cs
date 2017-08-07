using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class PaymentTypeModel
    {
        public int PaymentTypeKey { get; set; }
        public string PaymentTypeCode { get; set; }
        public string PaymentTypeName { get; set; }
        public string PaymentTypeDescription { get; set; }
    }
}